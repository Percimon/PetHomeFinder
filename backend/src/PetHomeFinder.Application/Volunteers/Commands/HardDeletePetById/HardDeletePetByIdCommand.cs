using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.Volunteers.Commands.HardDeletePetById;

public record HardDeletePetByIdCommand(Guid VolunteerId, Guid PetId) : ICommand;