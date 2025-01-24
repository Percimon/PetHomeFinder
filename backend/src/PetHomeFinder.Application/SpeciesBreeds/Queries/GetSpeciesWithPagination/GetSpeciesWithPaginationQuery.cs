using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.SpeciesBreeds.Queries.GetSpeciesWithPagination;

public record GetSpeciesWithPaginationQuery(
    int Page,
    int PageSize) : IQuery;