using PetHomeFinder.Core.Dtos;
using PetHomeFinder.Volunteers.Application.Commands.Create;

namespace PetHomeFinder.Volunteers.Contracts.Requests;

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