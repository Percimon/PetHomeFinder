using CSharpFunctionalExtensions;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.IDs;

namespace PetHomeFinder.Domain.Pets;

public class Breed : Shared.Entity<BreedId>
{
    private Breed(BreedId id) : base(id)
    {
    }
    private Breed(BreedId id, string breed) : base(id)
    {
        Value = breed;
    }

    public string Value { get; private set; }
    public static Result<Breed, Error> Create(BreedId id, string breed)
    {
        if (string.IsNullOrWhiteSpace(breed))
            return Errors.General.ValueIsRequired("Breed");

        if (breed.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsRequired("Breed");

        return new Breed(id, breed);
    }
}
