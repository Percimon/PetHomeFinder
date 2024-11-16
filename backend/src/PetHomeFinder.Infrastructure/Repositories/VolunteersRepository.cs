using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHomeFinder.Application.Volunteers;
using PetHomeFinder.Domain.PetManagement.AggregateRoot;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Infrastructure.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public VolunteersRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<Guid> Add(
        Volunteer volunteer,
        CancellationToken cancellationToken)
    {
        await _applicationDbContext.Volunteers.AddAsync(volunteer, cancellationToken);

        return volunteer.Id.Value;
    }

    public Guid Save(Volunteer volunteer)
    {
        _applicationDbContext.Volunteers.Attach(volunteer);

        return volunteer.Id.Value;
    }

    public Guid Delete(Volunteer volunteer)
    {
        _applicationDbContext.Volunteers.Remove(volunteer);

        return volunteer.Id.Value;
    }

    public async Task<Result<Volunteer, Error>> GetById(
        VolunteerId volunteerId,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _applicationDbContext.Volunteers
            .Include(v => v.PetsOwning)
            .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);

        if (volunteer is null)
            return Errors.General.NotFound(volunteerId);

        return volunteer;
    }
}