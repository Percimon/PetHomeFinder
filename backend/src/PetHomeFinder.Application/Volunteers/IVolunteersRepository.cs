using CSharpFunctionalExtensions;
using PetHomeFinder.Domain.PetManagement.AggregateRoot;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
    Guid Save(Volunteer volunteer);
    Guid Delete(Volunteer volunteer);
    Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken = default);
}
