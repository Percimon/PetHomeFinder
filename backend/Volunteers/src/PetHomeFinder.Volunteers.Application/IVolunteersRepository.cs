using CSharpFunctionalExtensions;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;
using PetHomeFinder.Volunteers.Domain.Entities;

namespace PetHomeFinder.Volunteers.Application;

public interface IVolunteersRepository
{
    Task<Guid> Add(
        Volunteer volunteer, 
        CancellationToken cancellationToken = default);
    
    Guid Save(Volunteer volunteer);
    
    Guid Delete(Volunteer volunteer);
    
    Task<Result<Volunteer, Error>> GetById(
        VolunteerId volunteerId, 
        CancellationToken cancellationToken = default);
}
