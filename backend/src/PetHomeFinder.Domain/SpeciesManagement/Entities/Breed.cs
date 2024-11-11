using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.IDs;

namespace PetHomeFinder.Domain.SpeciesManagement.Entities;

public class Breed : Shared.Entity<BreedId>
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
