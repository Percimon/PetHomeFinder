using PetHomeFinder.AnimalSpecies.Application.Queries.GetBreedsBySpeciesId;

namespace PetHomeFinder.AnimalSpecies.Contracts.Requests;

public record GetBreedsBySpeciesIdRequest(
    int Page,
    int PageSize)
{
    public GetBreedsBySpeciesIdQuery ToQuery(Guid speciesId) =>
        new(speciesId, Page, PageSize);
}