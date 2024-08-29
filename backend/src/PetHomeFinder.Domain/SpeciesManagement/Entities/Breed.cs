using System;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.IDs;

namespace PetHomeFinder.Domain.Pets;

public class Breed : Entity<BreedId>
{
    public Breed(BreedId id) : base(id)
    {
    }
}
