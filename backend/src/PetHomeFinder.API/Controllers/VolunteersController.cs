using Microsoft.AspNetCore.Mvc;
using PetHomeFinder.Application.Volunteers.CreateVolunteer;

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

    }
}