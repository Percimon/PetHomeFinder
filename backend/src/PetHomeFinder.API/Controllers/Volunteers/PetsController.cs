using Microsoft.AspNetCore.Mvc;
using PetHomeFinder.API.Controllers.Volunteers.Requests;
using PetHomeFinder.API.Extensions;
using PetHomeFinder.Application.Volunteers.Queries.GetPetsWithPagination;
using PetHomeFinder.Application.Volunteers.Queries.GetVolunteerById;

namespace PetHomeFinder.API.Controllers.Volunteers;

public class PetsController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> GetPetsWithPagination(
        [FromBody] GetPetsWithPaginationRequest request,
        [FromServices] GetPetsWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPetsWithPaginationQuery(
            request.VolunteerId,
            request.SpeciesId,
            request.BreedId,
            request.Name,
            request.Color,
            request.City,
            request.Street,
            request.HouseNumber,
            request.HeightFrom,
            request.HeightTo,
            request.WeightFrom,
            request.WeightTo,
            request.IsCastrated,
            request.Younger,
            request.Older,
            request.IsVaccinated,
            request.HelpStatus,
            request.SortBy,
            request.SortDirection,
            request.Page,
            request.PageSize);
            
        var result = await handler.Handle(query, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}