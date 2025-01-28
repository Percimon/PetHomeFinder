using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.DTOs;

namespace PetHomeFinder.Application.Volunteers.Commands.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<CredentialDto> Credentials,
    IEnumerable<SocialNetworkDto> SocialNetworks) : ICommand;
