namespace PetHomeFinder.Core.Dtos;

public class SpeciesDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; }

    public List<BreedDto> Breeds { get; init; } = [];
}