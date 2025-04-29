using CSharpFunctionalExtensions;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.Volunteers.Contracts;

public interface IPetsContract
{
    Task<UnitResult<ErrorList>> AnyPetIsOfSpecies(Guid speciesId, CancellationToken cancellationToken);
    
    Task<UnitResult<ErrorList>> AnyPetIsOfBreed(Guid breedId, CancellationToken cancellationToken);
}