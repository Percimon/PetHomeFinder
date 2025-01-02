using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.DTOs;

namespace PetHomeFinder.Application.Volunteers.Commands.UpdateCredentials;

public record UpdateCredentialsCommand(
    Guid VolunteerId,
    CredentialListDto CredentialList) : ICommand;