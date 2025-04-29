using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.Volunteers.Application.Commands.HardDeletePetById;

public record HardDeletePetByIdCommand(Guid VolunteerId, Guid PetId) : ICommand;