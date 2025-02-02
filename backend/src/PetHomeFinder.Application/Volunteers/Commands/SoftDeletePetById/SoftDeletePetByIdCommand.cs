using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.Volunteers.Commands.SoftDeletePetById;

public record SoftDeletePetByIdCommand(Guid VolunteerId, Guid PetId) : ICommand;