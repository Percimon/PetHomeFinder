using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Application.Volunteers.Commands.UpdateCredentials;

namespace PetHomeFinder.API.Controllers.Volunteers.Requests;

public record UpdateCredentialsRequest(IEnumerable<CredentialDto> Credentials)
{
    public UpdateCredentialsCommand ToCommand(Guid id) => new(id, Credentials);
}