using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHomeFinder.AnimalSpecies.Application;
using PetHomeFinder.AnimalSpecies.Application.Commands.AddBreed;
using PetHomeFinder.AnimalSpecies.Application.Commands.Create;
using PetHomeFinder.AnimalSpecies.Application.Queries.GetBreedsBySpeciesId;
using PetHomeFinder.AnimalSpecies.Application.Queries.GetSpeciesWithPagination;
using PetHomeFinder.AnimalSpecies.Contracts;
using PetHomeFinder.AnimalSpecies.Contracts.Requests;
using PetHomeFinder.Core.Dtos;
using PetHomeFinder.Core.Models;
using PetHomeFinder.Framework;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.AnimalSpecies.Presentation;

public class AnimalSpeciesContract : IAnimalSpeciesContract
{
    private readonly IReadDbContext _readContext;
    private readonly CreateSpeciesHandler _createSpeciesHandler;
    private readonly AddBreedHandler _addBreedHandler;
    private readonly GetSpeciesWithPaginationHandler _getSpeciesWithPaginationHandler;
    private readonly GetBreedsBySpeciesIdHandler _getBreedsBySpeciesIdHandler;

    public AnimalSpeciesContract(
        IReadDbContext readContext,
        CreateSpeciesHandler createSpeciesHandler,
        AddBreedHandler addBreedHandler,
        GetSpeciesWithPaginationHandler getSpeciesWithPaginationHandler,
        GetBreedsBySpeciesIdHandler getBreedsBySpeciesIdHandler)
    {
        _createSpeciesHandler = createSpeciesHandler;
        _addBreedHandler = addBreedHandler;
        _getSpeciesWithPaginationHandler = getSpeciesWithPaginationHandler;
        _getBreedsBySpeciesIdHandler = getBreedsBySpeciesIdHandler;
        _readContext = readContext;
    }

    public async Task<Result<PagedList<SpeciesDto>, ErrorList>> GetSpecies(
        GetSpeciesWithPaginationRequest request,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();

        var result = await _getSpeciesWithPaginationHandler.Handle(query, cancellationToken);
        if (result.IsFailure)
            return result.Error;

        return result.Value;
    }

    public async Task<Result<PagedList<BreedDto>, ErrorList>> GetBreedsBySpecies(
        Guid speciesId,
        GetBreedsBySpeciesIdRequest request,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery(speciesId);

        var result = await _getBreedsBySpeciesIdHandler.Handle(query, cancellationToken);
        if (result.IsFailure)
            return result.Error;

        return result.Value;
    }

    public async Task<Result<Guid, ErrorList>> CreateSpecies(
        CreateSpeciesRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();

        var result = await _createSpeciesHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error;

        return result.Value;
    }

    public async Task<Result<Guid, ErrorList>> AddBreed(
        Guid speciesId,
        AddBreedRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(speciesId);

        var result = await _addBreedHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error;

        return result.Value;
    }

    public async Task<UnitResult<ErrorList>> SpeciesExists(
        Guid speciesId,
        CancellationToken cancellationToken)
    {
        var speciesQuery = await _readContext.Species
            .FirstOrDefaultAsync(x => x.Id == speciesId, cancellationToken);
        
        if(speciesQuery is null)
            return Errors.General.NotFound(speciesId).ToErrorList();

        return UnitResult.Success<ErrorList>();
    }

    public async Task<UnitResult<ErrorList>> BreedExists(
        Guid speciesId, 
        Guid breedId, 
        CancellationToken cancellationToken)
    {
        var speciesQuery = await _readContext.Species
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(x => x.Id == speciesId, cancellationToken);
        
        if(speciesQuery is null)
            return Errors.General.NotFound(speciesId).ToErrorList();

        var breedResult = speciesQuery.Breeds
            .FirstOrDefault(b => b.Id == breedId);
        
        if(breedResult is null)
            return Errors.General.NotFound(breedId).ToErrorList();
        
        return UnitResult.Success<ErrorList>();
    }
}