namespace PetHomeFinder.Application.DTOs;

public class SpeciesDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; }

    public BreedDto[] Breeds { get; init; } = [];
}