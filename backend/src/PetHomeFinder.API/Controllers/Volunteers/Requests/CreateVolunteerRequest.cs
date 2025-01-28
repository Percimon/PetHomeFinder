using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Application.Volunteers.Commands.Create;

namespace PetHomeFinder.API.Controllers.Volunteers.Requests;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<CredentialDto> Credentials,
    IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public CreateVolunteerCommand ToCommand() =>
        new(FullName, Description, Experience, PhoneNumber, Credentials, SocialNetworks);
}