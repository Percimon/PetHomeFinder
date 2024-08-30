using System;
using CSharpFunctionalExtensions;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.IDs;

namespace PetHomeFinder.Domain.Pets;

public class Species : Shared.Entity<SpeciesId>
{
    private Species(SpeciesId id) : base(id)
    {
    }
    private Species(SpeciesId id, string species, IEnumerable<Breed> breedList) : base(id)
    {
        BreedList = breedList.ToList();
        Value = species;
    }

    public List<Breed> BreedList { get; private set; }
    public string Value { get; private set; }

    public static Result<Species, string> Create(SpeciesId id, string species, IEnumerable<Breed> breedList)
    {
        if (string.IsNullOrWhiteSpace(species))
            return "Species name cant be empty";

        if (species.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return "Species name is too long";

        return new Species(id, species, breedList);
    }
}
