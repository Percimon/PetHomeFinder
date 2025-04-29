using CSharpFunctionalExtensions;
using PetHomeFinder.Core.Shared;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;
using PetHomeFinder.AnimalSpecies.Domain.Entities;

namespace PetHomeFinder.AnimalSpecies.Application;

public interface ISpeciesRepository
{
    Task<Result<Guid, Error>> Add(Species species, CancellationToken cancellationToken = default);
    Guid Save(Species species);
    Guid Delete(Species species);
    Task<Result<Species, Error>> GetById(SpeciesId volunteerId, CancellationToken cancellationToken = default);
    Task<Result<Species, Error>> GetByName(Name name, CancellationToken cancellationToken = default);
}