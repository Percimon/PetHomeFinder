using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.SpeciesBreeds.Queries.GetBreedsBySpeciesId;

public record GetBreedsBySpeciesIdQuery(
    Guid SpeciesId,
    int Page,
    int PageSize) : IQuery;