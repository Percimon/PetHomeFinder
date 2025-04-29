using PetHomeFinder.Core.Dtos;

namespace PetHomeFinder.Volunteers.Contracts.Requests;

public record UpdateMainInfoRequest(
    FullNameDto FullName,
    string Description,
    int Experience,
    string PhoneNumber);