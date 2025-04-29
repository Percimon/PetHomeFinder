using PetHomeFinder.Core.Dtos;

namespace PetHomeFinder.Volunteers.Contracts.Requests;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<CredentialDto> Credentials,
    IEnumerable<SocialNetworkDto> SocialNetworks);