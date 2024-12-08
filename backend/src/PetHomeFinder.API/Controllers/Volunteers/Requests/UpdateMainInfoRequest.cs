using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Application.Volunteers.UpdateMainInfo;

namespace PetHomeFinder.API.Controllers.Volunteers.Requests;

public record UpdateMainInfoRequest(
    FullNameDto FullName,
    string Description,
    int Experience,
    string PhoneNumber)
{
    public UpdateMainInfoCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, FullName, Description, Experience, PhoneNumber);
}