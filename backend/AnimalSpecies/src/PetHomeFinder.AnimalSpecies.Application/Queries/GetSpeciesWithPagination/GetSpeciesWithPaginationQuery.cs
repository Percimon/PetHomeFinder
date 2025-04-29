using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.AnimalSpecies.Application.Queries.GetSpeciesWithPagination;

public record GetSpeciesWithPaginationQuery(
    int Page,
    int PageSize) : IQuery;