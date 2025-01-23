using PetHomeFinder.Application.SpeciesBreeds.Queries.GetBreedsBySpeciesId;

namespace PetHomeFinder.API.Controllers.Species.Requests;

public record GetBreedsBySpeciesIdRequest(
    int Page,
    int PageSize)
{
    public GetBreedsBySpeciesIdQuery ToQuery(Guid speciesId) =>
        new(speciesId, Page, PageSize);
}