using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.Volunteers.Commands.UpdatePetStatus;

public record UpdatePetStatusCommand(Guid VolunteerId, Guid PetId, string Status): ICommand;