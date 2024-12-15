using PetHomeFinder.Application.DTOs;

namespace PetHomeFinder.Application.Volunteers.Commands.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid VolunteerId,
    FullNameDto FullName,
    string Description,
    int Experience,
    string PhoneNumber);
