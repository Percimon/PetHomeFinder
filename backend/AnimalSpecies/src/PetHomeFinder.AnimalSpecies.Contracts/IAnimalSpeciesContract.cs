using CSharpFunctionalExtensions;
using PetHomeFinder.AnimalSpecies.Contracts.Requests;
using PetHomeFinder.Core.Dtos;
using PetHomeFinder.Core.Models;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.AnimalSpecies.Contracts;

public interface IAnimalSpeciesContract
{
    Task<Result<PagedList<SpeciesDto>, ErrorList>> GetSpecies(
        GetSpeciesWithPaginationRequest request,
        CancellationToken cancellationToken);

    Task<Result<PagedList<BreedDto>, ErrorList>> GetBreedsBySpecies(
        Guid speciesId,
        GetBreedsBySpeciesIdRequest request,
        CancellationToken cancellationToken);

    Task<Result<Guid, ErrorList>> CreateSpecies(
        CreateSpeciesRequest request,
        CancellationToken cancellationToken);

    Task<Result<Guid, ErrorList>> AddBreed(
        Guid speciesId,
        AddBreedRequest request,
        CancellationToken cancellationToken);
    
    Task<UnitResult<ErrorList>> SpeciesExists(
        Guid speciesId,
        CancellationToken cancellationToken);
    
    Task<UnitResult<ErrorList>> BreedExists(
        Guid speciesId,
        Guid breedId,
        CancellationToken cancellationToken);
}