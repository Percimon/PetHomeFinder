using System;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.IDs;

namespace PetHomeFinder.Domain.Pets;

public class Species : Entity<SpeciesId>
{
    public Species(SpeciesId id) : base(id)
    {
    }
    public Species(SpeciesId id, IEnumerable<Breed> breedList) : base(id)
    {
        BreedList = breedList.ToList();
    }

    public List<Breed> BreedList { get; private set; }
}
