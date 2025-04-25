using PetHomeFinder.Volunteers.Application.Commands.UpdatePetStatus;

namespace PetHomeFinder.Volunteers.Contracts.Requests;

public record UpdatePetStatusRequest(string Status)
{
    public UpdatePetStatusCommand ToCommand(Guid volunteerId, Guid petId) =>
        new(volunteerId, petId, Status);
}