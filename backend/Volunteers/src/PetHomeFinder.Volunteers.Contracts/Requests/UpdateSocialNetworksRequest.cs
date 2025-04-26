using PetHomeFinder.Core.Dtos;

namespace PetHomeFinder.Volunteers.Contracts.Requests;

public record UpdateSocialNetworksRequest(IEnumerable<SocialNetworkDto> SocialNetworkList);