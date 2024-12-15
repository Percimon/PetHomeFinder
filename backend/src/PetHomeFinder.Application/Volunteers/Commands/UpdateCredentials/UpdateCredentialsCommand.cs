using PetHomeFinder.Application.DTOs;

namespace PetHomeFinder.Application.Volunteers.Commands.UpdateCredentials;

public record UpdateCredentialsCommand(Guid VolunteerId, CredentialListDto CredentialList);
