using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.Volunteers.Application;
using PetHomeFinder.Volunteers.Contracts;

namespace PetHomeFinder.Volunteers.Presentation;

public class PetsContract : IPetsContract
{
    private readonly IVolunteersReadDbContext _volunteersReadDbContext;

    public PetsContract(IVolunteersReadDbContext volunteersReadDbContext)
    {
        _volunteersReadDbContext = volunteersReadDbContext;
    }

    public async Task<UnitResult<ErrorList>> AnyPetIsOfSpecies(
        Guid speciesId, 
        CancellationToken cancellationToken)
    {
        var petQuery = await _volunteersReadDbContext.Pets
            .FirstOrDefaultAsync(pet => pet.SpeciesId == speciesId, cancellationToken);
        
        if (petQuery is not null)
        {
            return Errors.General.IsUsed("Species", speciesId).ToErrorList();
        }

        return UnitResult.Success<ErrorList>();
    }

    public async Task<UnitResult<ErrorList>> AnyPetIsOfBreed(Guid breedId, CancellationToken cancellationToken)
    {
        var petQuery = await _volunteersReadDbContext.Pets
            .FirstOrDefaultAsync(pet => pet.BreedId == breedId, cancellationToken);
        
        if (petQuery is not null)
        {
            return Errors.General.IsUsed("Breed", breedId).ToErrorList();
        }

        return UnitResult.Success<ErrorList>();
    }
}