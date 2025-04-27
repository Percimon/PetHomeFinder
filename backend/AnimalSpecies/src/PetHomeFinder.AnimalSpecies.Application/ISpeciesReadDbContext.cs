using PetHomeFinder.Core.Dtos;

namespace PetHomeFinder.AnimalSpecies.Application;

public interface ISpeciesReadDbContext
{
    IQueryable<SpeciesDto> Species { get; }

    IQueryable<BreedDto> Breeds { get; }
}