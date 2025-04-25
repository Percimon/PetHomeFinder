using PetHomeFinder.Core.Dtos;

namespace PetHomeFinder.Volunteers.Application;

public interface IReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }

    IQueryable<PetDto> Pets { get; }
}