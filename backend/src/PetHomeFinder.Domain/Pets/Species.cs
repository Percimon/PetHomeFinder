using System;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.Pets;

public class Species : Entity
{
    public Species(Guid id) : base(id)
    {
    }

    public List<Breed> BreedList { get; private set; }
}
