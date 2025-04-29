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
            [FromQuery] GetVolunteersWithPaginationRequest request,
            [FromServices] GetVolunteersWithPaginationHandler handler,
            CancellationToken cancellationToken = default)
        {
            var query = new GetVolunteersWithPaginationQuery(
                request.FirstName,
                request.LastName,
                request.Surname,
                request.Page,
                request.PageSize);

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
            var command = new CreateVolunteerCommand(
                request.FullName,
                request.Description,
                request.Experience,
                request.PhoneNumber,
                request.Credentials,
                request.SocialNetworks);

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
            var command = new UpdateMainInfoCommand(
                id,
                request.FullName,
                request.Description,
                request.Experience,
                request.PhoneNumber);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPut("{id:guid}/credentials")]
        public async Task<ActionResult> UpdateCredentials(
            [FromRoute] Guid id,
            [FromBody] UpdateCredentialsRequest request,
            [FromServices] UpdateCredentialsHandler handler,
            CancellationToken cancellationToken = default)
        {
            var command = new UpdateCredentialsCommand(id, request.Credentials);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPut("{id:guid}/social-networks")]
        public async Task<ActionResult> UpdateSocialNetworks(
            [FromRoute] Guid id,
            [FromBody] UpdateSocialNetworksRequest request,
            [FromServices] UpdateSocialNetworksHandler handler,
            CancellationToken cancellationToken = default)
        {
            var command = new UpdateSocialNetworksCommand(id, request.SocialNetworkList);

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
            [FromBody] AddPetRequest request,
            [FromServices] AddPetHandler handler,
            CancellationToken cancellationToken = default)
        {
            var command = new AddPetCommand(id,
                request.Name,
                request.SpeciesId,
                request.BreedId,
                request.Description,
                request.Color,
                request.HealthInfo,
                request.Address,
                request.Weight,
                request.Height,
                request.OwnerPhoneNumber,
                request.IsCastrated,
                request.IsVaccinated,
                request.BirthDate,
                request.HelpStatus,
                request.Credentials,
                request.CreateDate);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpPost("{id:guid}/pet/{petId:guid}")]
        public async Task<ActionResult<Guid>> UpdatePet(
            [FromRoute] Guid id,
            [FromRoute] Guid petId,
            [FromBody] UpdatePetRequest request,
            [FromServices] UpdatePetHandler handler,
            CancellationToken cancellationToken = default)
        {
            var command = new UpdatePetCommand(id,
                petId,
                request.SpeciesId,
                request.BreedId,
                request.Name,
                request.Description,
                request.Color,
                request.HealthInfo,
                request.Address,
                request.Weight,
                request.Height,
                request.PhoneNumber,
                request.IsCastrated,
                request.IsVaccinated,
                request.BirthDate,
                request.Credentials);

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
            var command = new UpdatePetStatusCommand(id, petId, request.Status);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}/pet/{petId:guid}/soft")]
        public async Task<ActionResult> SoftDeletePet(
            [FromRoute] Guid id,
            [FromRoute] Guid petId,
            [FromServices] SoftDeletePetByIdHandler handler,
            CancellationToken cancellationToken = default)
        {
            var command = new SoftDeletePetByIdCommand(id, petId);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}/pet/{petId:guid}/hard")]
        public async Task<ActionResult> HardDeletePet(
            [FromRoute] Guid id,
            [FromRoute] Guid petId,
            [FromServices] HardDeletePetByIdHandler handler,
            CancellationToken cancellationToken = default)
        {
            var command = new HardDeletePetByIdCommand(id, petId);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPost("{id:guid}/pet/{petId:guid}/files")]
        public async Task<ActionResult> UploadFilesToPet(
            [FromRoute] Guid id,
            [FromRoute] Guid petId,
            [FromForm] IFormFileCollection files,
            [FromServices] UploadFilesToPetHandler handler,
            CancellationToken cancellationToken)
        {
            await using var fileProcessor = new FormFileProcessor();
            var fileDtos = fileProcessor.ToUploadFileDtos(files);

            var command = new UploadFilesToPetCommand(id, petId, fileDtos);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPost("{id:guid}/pet/{petId:guid}/main-photo")]
        public async Task<ActionResult> UpdatePetMainPhoto(
            [FromRoute] Guid id,
            [FromRoute] Guid petId,
            [FromBody] UpdatePetMainPhotoRequest request,
            [FromServices] UpdatePetMainPhotoHandler handler,
            CancellationToken cancellationToken = default)
        {
            var command = new UpdatePetMainPhotoCommand(id, petId, request.FilePath);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}