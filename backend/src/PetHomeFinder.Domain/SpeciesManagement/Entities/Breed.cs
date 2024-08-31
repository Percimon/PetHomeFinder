using System;
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
    public static Result<Breed, string> Create(BreedId id, string breed)
    {
        if (string.IsNullOrWhiteSpace(breed))
            return "Breed name cant be empty";

        if (breed.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return "Breed name is too long";

        return new Breed(id, breed);
    }
}
