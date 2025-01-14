namespace PetHomeFinder.Application.DTOs;

public class PetDto
{
    public Guid Id { get; init; }

    public Guid SpeciesId { get; init; }

    public Guid BreedId { get; init; }

    public Guid VolunteerId { get; init; }
}