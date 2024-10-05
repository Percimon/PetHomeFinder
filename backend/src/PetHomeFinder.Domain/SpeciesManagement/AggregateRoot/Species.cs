using CSharpFunctionalExtensions;
using PetHomeFinder.Domain.Pets;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.IDs;

namespace PetHomeFinder.Domain.SpeciesManagement.AggregateRoot;

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

    public static Result<Species, Error> Create(SpeciesId id, string species, IEnumerable<Breed> breedList)
    {
        if (string.IsNullOrWhiteSpace(species))
            return Errors.General.ValueIsRequired("Species");

        if (species.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsRequired("Species");

        return new Species(id, species, breedList);
    }
}
