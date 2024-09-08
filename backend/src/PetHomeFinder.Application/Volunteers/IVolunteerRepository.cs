using System;
using PetHomeFinder.Domain.Volunteers;

namespace PetHomeFinder.Application.Volunteers;

public interface IVolunteerRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
}
