using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.AnimalSpecies.Application.Queries.GetBreedsBySpeciesId;

public record GetBreedsBySpeciesIdQuery(
    Guid SpeciesId,
    int Page,
    int PageSize) : IQuery;