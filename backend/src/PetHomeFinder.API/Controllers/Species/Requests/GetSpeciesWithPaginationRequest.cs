using PetHomeFinder.Application.SpeciesBreeds.Queries.GetSpeciesWithPagination;

namespace PetHomeFinder.API.Controllers.Species.Requests;

public record GetSpeciesWithPaginationRequest(
    int Page,
    int PageSize)
{
    public GetSpeciesWithPaginationQuery ToQuery() =>
        new GetSpeciesWithPaginationQuery(Page, PageSize);
}