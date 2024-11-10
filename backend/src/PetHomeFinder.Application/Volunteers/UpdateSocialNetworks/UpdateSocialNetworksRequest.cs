using PetHomeFinder.Application.DTOs;

namespace PetHomeFinder.Application.Volunteers.UpdateSocialNetworks;

public record UpdateSocialNetworksRequest(Guid VolunteerId, SocialNetworkListDto SocialNetworkList);

