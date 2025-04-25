using PetHomeFinder.Core.Dtos;
using PetHomeFinder.Volunteers.Application.Commands.UpdateSocialNetworks;

namespace PetHomeFinder.Volunteers.Contracts.Requests;

public record UpdateSocialNetworksRequest(IEnumerable<SocialNetworkDto> SocialNetworkList)
{
    public UpdateSocialNetworksCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, SocialNetworkList);
}