using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Application.Volunteers.UpdateCredentials;

namespace PetHomeFinder.API.Controllers.Volunteers.Requests;

public record UpdateCredentialsRequest(CredentialListDto CredentialList)
{
    public UpdateCredentialsCommand ToCommand(Guid id) => new(id, CredentialList);
}