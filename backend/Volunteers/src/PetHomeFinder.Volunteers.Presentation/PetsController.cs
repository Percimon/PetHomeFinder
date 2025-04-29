using Microsoft.AspNetCore.Mvc;
using PetHomeFinder.Framework;
using PetHomeFinder.Volunteers.Application.Queries.GetPetById;
using PetHomeFinder.Volunteers.Application.Queries.GetPetsWithPagination;
using PetHomeFinder.Volunteers.Contracts.Requests;

namespace PetHomeFinder.Volunteers.Presentation;

public class PetsController : ApplicationController
{
    [HttpGet("pets")]
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
    
    [HttpGet("pets/{petId:guid}")]
    public async Task<ActionResult> GetPetById(
        [FromRoute] Guid petId,
        [FromServices] GetPetByIdHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPetByIdQuery(petId);
            
        var result = await handler.Handle(query, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}