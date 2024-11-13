using PetHomeFinder.Application.DTOs;

namespace PetHomeFinder.Application.Volunteers.UpdateSocialNetworks;

public record UpdateSocialNetworksCommand(Guid VolunteerId, SocialNetworkListDto SocialNetworkList);

