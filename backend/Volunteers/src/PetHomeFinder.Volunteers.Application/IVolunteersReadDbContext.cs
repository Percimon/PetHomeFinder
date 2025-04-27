using PetHomeFinder.Core.Dtos;

namespace PetHomeFinder.Volunteers.Application;

public interface IVolunteersReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }

    IQueryable<PetDto> Pets { get; }
}