using PetHomeFinder.Core.Shared;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;

namespace PetHomeFinder.AnimalSpecies.Domain.Entities;

public class Breed : Entity<BreedId>
{
    private Breed(BreedId id) : base(id)
    {
    }
    
    public Breed(
        BreedId id, 
        Name name) : base(id)
    {
        Name = name;
    }

    public Name Name { get; private set; }
    
}
