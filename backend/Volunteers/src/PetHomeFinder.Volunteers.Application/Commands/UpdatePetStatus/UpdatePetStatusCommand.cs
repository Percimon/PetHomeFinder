using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.Volunteers.Application.Commands.UpdatePetStatus;

public record UpdatePetStatusCommand(Guid VolunteerId, Guid PetId, string Status): ICommand;