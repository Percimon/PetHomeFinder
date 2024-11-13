using PetHomeFinder.Application.DTOs;

namespace PetHomeFinder.Application.Volunteers.UpdateCredentials;

public record UpdateCredentialsCommand(Guid VolunteerId, CredentialListDto CredentialList);
