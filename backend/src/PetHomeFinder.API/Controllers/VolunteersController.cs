using Microsoft.AspNetCore.Mvc;
using PetHomeFinder.API.Extensions;
using PetHomeFinder.Application.Volunteers.Create;
using PetHomeFinder.Application.Volunteers.Delete;
using PetHomeFinder.Application.Volunteers.UpdateCredentials;
using PetHomeFinder.Application.Volunteers.UpdateMainInfo;
using PetHomeFinder.Application.Volunteers.UpdateSocialNetworks;

namespace PetHomeFinder.API.Controllers
{

    public class VolunteersController : ApplicationController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
                [FromServices] CreateVolunteerHandler handler,
                [FromBody] CreateVolunteerRequest request,
                CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(request, cancellationToken);
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
            var command = new UpdateMainInfoRequest(
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
            [FromServices] UpdateCredentialsHandler handler,
            [FromBody] UpdateCredentialsRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = new UpdateCredentialsRequest(id, request.CredentialList);

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
            var command = new UpdateSocialNetworksRequest(id, request.SocialNetworkList);

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
            var request = new DeleteVolunteerRequest(id);

            var result = await handler.Handle(request, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

    }
}