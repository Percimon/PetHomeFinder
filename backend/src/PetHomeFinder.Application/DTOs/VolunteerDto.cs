namespace PetHomeFinder.Application.DTOs;

public class VolunteerDto
{
    public Guid Id { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Surname { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public int Experience { get; init; }

    public string PhoneNumber { get; init; } = string.Empty;

    public CredentialDto[] Credentials { get; set; } = [];

    public SocialNetworkDto[] SocialNetworks { get; set; } = [];

    public PetDto[] Pets { get; init; } = [];

    public bool IsDeleted { get; init; }
}