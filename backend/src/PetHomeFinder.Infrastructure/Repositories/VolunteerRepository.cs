using System;
using PetHomeFinder.Domain.Volunteers;

namespace PetHomeFinder.Infrastructure.Repositories;

public class VolunteerRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public VolunteerRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken)
    {
        await _applicationDbContext.Volunteers.AddAsync(volunteer, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return volunteer.Id.Value;
    }

}
