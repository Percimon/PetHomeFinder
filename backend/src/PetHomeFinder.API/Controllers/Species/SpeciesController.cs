using Microsoft.AspNetCore.Mvc;
using PetHomeFinder.API.Controllers.Species.Requests;
using PetHomeFinder.API.Extensions;
using PetHomeFinder.Application.SpeciesBreeds.Commands.AddBreed;
using PetHomeFinder.Application.SpeciesBreeds.Commands.Create;
using PetHomeFinder.Application.SpeciesBreeds.Commands.Delete;
using PetHomeFinder.Application.SpeciesBreeds.Commands.DeleteBreed;
using PetHomeFinder.Application.SpeciesBreeds.Queries.GetBreedsBySpeciesId;
using PetHomeFinder.Application.SpeciesBreeds.Queries.GetSpeciesWithPagination;

namespace PetHomeFinder.API.Controllers.Species;

public class SpeciesController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> GetSpeciesWithPagination(
        [FromServices] GetSpeciesWithPaginationHandler handler,
        [FromQuery] GetSpeciesWithPaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();

        var result = await handler.Handle(query, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpGet("{speciesId:guid}")]
    public async Task<ActionResult> GetBreedsBySpeciesId(
        [FromRoute] Guid speciesId,
        [FromServices] GetBreedsBySpeciesIdHandler handler,
        [FromQuery] GetBreedsBySpeciesIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery(speciesId);

        var result = await handler.Handle(query, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateSpeciesHandler handler,
        [FromBody] CreateSpeciesRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand();

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost("{speciesId:guid}/breed")]
    public async Task<ActionResult<Guid>> AddBreed(
        [FromRoute] Guid speciesId,
        [FromServices] AddBreedHandler handler,
        [FromBody] AddBreedRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(speciesId);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteSpeciesHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteSpeciesCommand(id);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{speciesId:guid}/breed/{breedId:guid}")]
    public async Task<ActionResult> DeleteBreed(
        [FromRoute] Guid speciesId,
        [FromRoute] Guid breedId,
        [FromServices] DeleteBreedHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteBreedCommand(speciesId, breedId);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}