using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Application.Volunteers.Create;

namespace PetHomeFinder.API.Contracts;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int Experience,
    string PhoneNumber,
    CredentialListDto CredentialList,
    SocialNetworkListDto SocialNetworkList)
{
    public CreateVolunteerCommand ToCommand() =>
        new(FullName, Description, Experience, PhoneNumber, CredentialList, SocialNetworkList);
}