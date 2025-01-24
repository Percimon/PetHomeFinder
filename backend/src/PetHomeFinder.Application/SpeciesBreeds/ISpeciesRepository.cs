using CSharpFunctionalExtensions;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.AggregateRoot;
using PetHomeFinder.Domain.SpeciesManagement.IDs;


namespace PetHomeFinder.Application.SpeciesBreeds;

public interface ISpeciesRepository
{
    Task<Result<Guid, Error>> Add(Species species, CancellationToken cancellationToken = default);
    Guid Save(Species species);
    Guid Delete(Species species);
    Task<Result<Species, Error>> GetById(SpeciesId volunteerId, CancellationToken cancellationToken = default);
    Task<Result<Species, Error>> GetByName(Name name, CancellationToken cancellationToken = default);
}