using PetHomeFinder.Core.Dtos;

namespace PetHomeFinder.AnimalSpecies.Application;

public interface IReadDbContext
{
    IQueryable<SpeciesDto> Species { get; }

    IQueryable<BreedDto> Breeds { get; }
}