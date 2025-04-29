using CSharpFunctionalExtensions;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;

namespace PetHomeFinder.Volunteers.Domain.ValueObjects;

public record SpeciesBreed
{
    private SpeciesBreed(SpeciesId speciesId, Guid breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }

    public SpeciesId SpeciesId { get; }

    public Guid BreedId { get; }

    public static Result<SpeciesBreed, Error> Create(SpeciesId speciesId, Guid breedId)
    {
        return new SpeciesBreed(speciesId, breedId);
    }
}