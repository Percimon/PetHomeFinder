using PetHomeFinder.Application.DTOs;

namespace PetHomeFinder.Application.Volunteers.Commands.UpdateSocialNetworks;

public record UpdateSocialNetworksCommand(Guid VolunteerId, SocialNetworkListDto SocialNetworkList);

