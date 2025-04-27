using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;
using PetHomeFinder.Volunteers.Application;
using PetHomeFinder.Volunteers.Domain.Entities;
using PetHomeFinder.Volunteers.Infrastructure.DbContexts;

namespace PetHomeFinder.Volunteers.Infrastructure;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly VolunteersWriteDbContext _volunteersWriteDbContext;

    public VolunteersRepository(VolunteersWriteDbContext volunteersWriteDbContext)
    {
        _volunteersWriteDbContext = volunteersWriteDbContext;
    }

    public async Task<Guid> Add(
        Volunteer volunteer,
        CancellationToken cancellationToken)
    {
        await _volunteersWriteDbContext.Volunteers.AddAsync(volunteer, cancellationToken);

        return volunteer.Id.Value;
    }

    public Guid Save(Volunteer volunteer)
    {
        _volunteersWriteDbContext.Volunteers.Attach(volunteer);

        return volunteer.Id.Value;
    }

    public Guid Delete(Volunteer volunteer)
    {
        _volunteersWriteDbContext.Volunteers.Remove(volunteer);

        return volunteer.Id.Value;
    }

    public async Task<Result<Volunteer, Error>> GetById(
        VolunteerId volunteerId,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _volunteersWriteDbContext.Volunteers
            .Include(v => v.PetsOwning)
            .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);

        if (volunteer is null)
            return Errors.General.NotFound(volunteerId);

        return volunteer;
    }
}