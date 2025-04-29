using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Dtos;

namespace PetHomeFinder.Volunteers.Application.Commands.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<CredentialDto> Credentials,
    IEnumerable<SocialNetworkDto> SocialNetworks) : ICommand;
