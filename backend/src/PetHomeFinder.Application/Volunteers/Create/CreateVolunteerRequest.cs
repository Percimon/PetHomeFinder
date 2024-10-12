using PetHomeFinder.Application.DTOs;

namespace PetHomeFinder.Application.Volunteers.Create;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int Experience,
    string PhoneNumber,
    CredentialListDto CredentialList,
    SocialNetworkListDto SocialNetworkList);
