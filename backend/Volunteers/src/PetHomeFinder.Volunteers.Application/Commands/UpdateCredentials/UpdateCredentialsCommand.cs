using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Dtos;

namespace PetHomeFinder.Volunteers.Application.Commands.UpdateCredentials;

public record UpdateCredentialsCommand(
    Guid VolunteerId,
    IEnumerable<CredentialDto> Credentials) : ICommand;