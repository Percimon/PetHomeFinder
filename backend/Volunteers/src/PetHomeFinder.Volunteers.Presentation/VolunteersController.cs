using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetHomeFinder.Framework;
using PetHomeFinder.Volunteers.Application.Commands.AddPet;
using PetHomeFinder.Volunteers.Application.Commands.Create;
using PetHomeFinder.Volunteers.Application.Commands.Delete;
using PetHomeFinder.Volunteers.Application.Commands.HardDeletePetById;
using PetHomeFinder.Volunteers.Application.Commands.SoftDeletePetById;
using PetHomeFinder.Volunteers.Application.Commands.UpdateCredentials;
using PetHomeFinder.Volunteers.Application.Commands.UpdateMainInfo;
using PetHomeFinder.Volunteers.Application.Commands.UpdatePet;
using PetHomeFinder.Volunteers.Application.Commands.UpdatePetMainPhoto;
using PetHomeFinder.Volunteers.Application.Commands.UpdatePetStatus;
using PetHomeFinder.Volunteers.Application.Commands.UpdateSocialNetworks;
using PetHomeFinder.Volunteers.Application.Commands.UploadFilesToPet;
using PetHomeFinder.Volunteers.Application.Queries.GetVolunteerById;
using PetHomeFinder.Volunteers.Application.Queries.GetVolunteersWithPagination;
using PetHomeFinder.Volunteers.Contracts.Requests;
using PetHomeFinder.Volunteers.Presentation.Processors;

namespace PetHomeFinder.Volunteers.Presentation
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
        
        [HttpPost("{id:guid}/pet/{petId:guid}/status")]
        public async Task<ActionResult<Guid>> UpdatePetStatus(
            [FromRoute] Guid id,
            [FromRoute] Guid petId,
            [FromServices] UpdatePetStatusHandler handler,
            [FromBody] UpdatePetStatusRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = request.ToCommand(id, petId);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }
        
        [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/soft")]
        public async Task<ActionResult> SoftDeletePet(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromServices] SoftDeletePetByIdHandler handler,
            CancellationToken cancellationToken = default)
        {
            var command = new SoftDeletePetByIdCommand(volunteerId, petId);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/hard")]
        public async Task<ActionResult> HardDeletePet(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromServices] HardDeletePetByIdHandler handler,
            CancellationToken cancellationToken = default)
        {
            var command = new HardDeletePetByIdCommand(volunteerId, petId);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

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
        
        [HttpPost("{volunteerId:guid}/pet/{petId:guid}/main-photo")]
        public async Task<ActionResult> UpdatePetMainPhoto(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromBody] UpdatePetMainPhotoRequest request,
            [FromServices] UpdatePetMainPhotoHandler handler,
            CancellationToken cancellationToken = default)
        {
            var command = request.ToCommand(volunteerId, petId);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

    }
}