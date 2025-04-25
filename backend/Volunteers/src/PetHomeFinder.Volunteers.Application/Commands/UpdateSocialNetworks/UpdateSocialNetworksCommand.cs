using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Dtos;

namespace PetHomeFinder.Volunteers.Application.Commands.UpdateSocialNetworks;

public record UpdateSocialNetworksCommand(
    Guid VolunteerId,
    IEnumerable<SocialNetworkDto> SocialNetworks) : ICommand;