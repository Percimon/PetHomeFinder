using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Application.Volunteers.UpdateSocialNetworks;

namespace PetHomeFinder.API.Controllers.Volunteers.Requests;

public record UpdateSocialNetworksRequest(SocialNetworkListDto SocialNetworkList)
{
    public UpdateSocialNetworksCommand ToCommand(Guid volunteerId) => new(volunteerId, SocialNetworkList); 
}
