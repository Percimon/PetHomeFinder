using PetHomeFinder.Application.DTOs;

namespace PetHomeFinder.Application.Volunteers.UpdateCredentials;

public record UpdateCredentialsRequest(Guid VolunteerId, CredentialListDto CredentialList);
