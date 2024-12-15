namespace PetHomeFinder.Application.DTOs;

public class VolunteerDto
{
    public Guid Id { get; init; }

    public PetDto[] Pets { get; init; } = [];
}