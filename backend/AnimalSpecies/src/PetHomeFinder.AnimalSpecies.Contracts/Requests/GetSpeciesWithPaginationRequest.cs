using PetHomeFinder.AnimalSpecies.Application.Queries.GetSpeciesWithPagination;

namespace PetHomeFinder.AnimalSpecies.Contracts.Requests;

public record GetSpeciesWithPaginationRequest(
    int Page,
    int PageSize)
{
    public GetSpeciesWithPaginationQuery ToQuery() =>
        new GetSpeciesWithPaginationQuery(Page, PageSize);
}