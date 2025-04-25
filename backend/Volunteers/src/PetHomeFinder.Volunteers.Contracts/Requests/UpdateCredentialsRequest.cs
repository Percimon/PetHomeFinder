using PetHomeFinder.Core.Dtos;
using PetHomeFinder.Volunteers.Application.Commands.UpdateCredentials;

namespace PetHomeFinder.Volunteers.Contracts.Requests;

public record UpdateCredentialsRequest(IEnumerable<CredentialDto> Credentials)
{
    public UpdateCredentialsCommand ToCommand(Guid id) => new(id, Credentials);
}