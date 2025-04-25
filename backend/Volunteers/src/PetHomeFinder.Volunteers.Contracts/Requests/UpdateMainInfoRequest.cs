using PetHomeFinder.Core.Dtos;
using PetHomeFinder.Volunteers.Application.Commands.UpdateMainInfo;

namespace PetHomeFinder.Volunteers.Contracts.Requests;

public record UpdateMainInfoRequest(
    FullNameDto FullName,
    string Description,
    int Experience,
    string PhoneNumber)
{
    public UpdateMainInfoCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, FullName, Description, Experience, PhoneNumber);
}