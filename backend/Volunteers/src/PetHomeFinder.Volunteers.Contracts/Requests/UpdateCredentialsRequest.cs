using PetHomeFinder.Core.Dtos;

namespace PetHomeFinder.Volunteers.Contracts.Requests;

public record UpdateCredentialsRequest(IEnumerable<CredentialDto> Credentials);