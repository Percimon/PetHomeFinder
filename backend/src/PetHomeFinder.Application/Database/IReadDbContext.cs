using PetHomeFinder.Application.DTOs;

namespace PetHomeFinder.Application.Database;

public interface IReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }
    IQueryable<PetDto> Pets { get; }
}