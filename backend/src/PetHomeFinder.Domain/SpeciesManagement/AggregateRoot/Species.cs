using CSharpFunctionalExtensions;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.Entities;
using PetHomeFinder.Domain.SpeciesManagement.IDs;

namespace PetHomeFinder.Domain.SpeciesManagement.AggregateRoot;

public class Species : Shared.Entity<SpeciesId>
{
    private readonly List<Breed> _breeds;
    
    private Species(SpeciesId id) : base(id)
    {
    }
    
    public Species(
        SpeciesId id, 
        Name name) : base(id)
    {
        Name = name;
        _breeds = new List<Breed>();
    }
    
    public Name Name { get; private set; }

    public IReadOnlyList<Breed> Breeds => _breeds;

    public Result<Guid, Error> AddBreed(Breed breed)
    {
        var result = _breeds.FirstOrDefault(b => b.Name == breed.Name);
        
        if (result is not null)
            return Errors.General.AlreadyExists(
                nameof(Breed), 
                nameof(breed.Name).ToLower(), 
                breed.Name.Value);

        _breeds.Add(breed);

        return breed.Id.Value;
    }

    public Result<Guid, Error> DeleteBreed(Breed breed)
    {
        _breeds.Remove(breed);

        return breed.Id.Value;
    }
}
