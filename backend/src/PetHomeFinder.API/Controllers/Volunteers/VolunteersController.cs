using Microsoft.AspNetCore.Mvc;
using PetHomeFinder.API.Controllers.Volunteers.Requests;
using PetHomeFinder.API.Extensions;
using PetHomeFinder.API.Processors;
using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Application.Models;
using PetHomeFinder.Application.Volunteers.Commands.AddPet;
using PetHomeFinder.Application.Volunteers.Commands.Create;
using PetHomeFinder.Application.Volunteers.Commands.Delete;
using PetHomeFinder.Application.Volunteers.Commands.UpdateCredentials;
using PetHomeFinder.Application.Volunteers.Commands.UpdateMainInfo;
using PetHomeFinder.Application.Volunteers.Commands.UpdatePet;
using PetHomeFinder.Application.Volunteers.Commands.UpdateSocialNetworks;
using PetHomeFinder.Application.Volunteers.Commands.UploadFilesToPet;
using PetHomeFinder.Application.Volunteers.Queries.GetVolunteerById;
using PetHomeFinder.Application.Volunteers.Queries.GetVolunteersWithPagination;

namespace PetHomeFinder.API.Controllers.Volunteers
{
    public class VolunteersController : ApplicationController
    {
        [HttpGet]
        public async Task<ActionResult> GetVolunteersWithPagination(
            [FromServices] GetVolunteersWithPaginationHandler handler,
            [FromQuery] GetVolunteersWithPaginationRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = request.ToQuery();

            var result = await handler.Handle(query, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetVolunteerById(
            [FromRoute] Guid id,
            [FromServices] GetVolunteerByIdHandler handler,
            CancellationToken cancellationToken = default)
        {
            var query = new GetVolunteerByIdQuery(id);
            
            var result = await handler.Handle(query, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
        
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromServices] CreateVolunteerHandler handler,
            [FromBody] CreateVolunteerRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = request.ToCommand();

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpPut("{id:guid}/main-info")]
        public async Task<ActionResult> UpdateMainInfo(
            [FromRoute] Guid id,
            [FromServices] UpdateMainInfoHandler handler,
            [FromBody] UpdateMainInfoRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = request.ToCommand(id);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPut("{id:guid}/credentials")]
        public async Task<ActionResult> UpdateCredentials(
            [FromRoute] Guid id,
            [FromServices] UpdateCredentialsHandler handler,
            [FromBody] UpdateCredentialsRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = request.ToCommand(id);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPut("{id:guid}/social-networks")]
        public async Task<ActionResult> UpdateSocialNetworks(
            [FromRoute] Guid id,
            [FromServices] UpdateSocialNetworksHandler handler,
            [FromBody] UpdateSocialNetworksRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = request.ToCommand(id);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(
            [FromRoute] Guid id,
            [FromServices] DeleteVolunteerHandler handler,
            CancellationToken cancellationToken = default)
        {
            var command = new DeleteVolunteerCommand(id);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPost("{id:guid}/pet")]
        public async Task<ActionResult<Guid>> AddPet(
            [FromRoute] Guid id,
            [FromServices] AddPetHandler handler,
            [FromBody] AddPetRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = request.ToCommand(id);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }
        
        [HttpPost("{id:guid}/pet/{petId:guid}")]
        public async Task<ActionResult<Guid>> UpdatePet(
            [FromRoute] Guid id,
            [FromRoute] Guid petId,
            [FromServices] UpdatePetHandler handler,
            [FromBody] UpdatePetRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = request.ToCommand(id, petId);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpPost("{volunteerId:guid}/pet/{petId:guid}/files")]
        public async Task<ActionResult> UploadFilesToPet(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromForm] IFormFileCollection files,
            [FromServices] UploadFilesToPetHandler handler,
            CancellationToken cancellationToken)
        {
            await using var fileProcessor = new FormFileProcessor();
            var fileDtos = fileProcessor.ToUploadFileDtos(files);

            var command = new UploadFilesToPetCommand(volunteerId, petId, fileDtos);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}