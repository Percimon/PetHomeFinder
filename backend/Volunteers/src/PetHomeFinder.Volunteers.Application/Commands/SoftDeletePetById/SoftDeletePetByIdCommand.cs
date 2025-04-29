using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.Volunteers.Application.Commands.SoftDeletePetById;

public record SoftDeletePetByIdCommand(Guid VolunteerId, Guid PetId) : ICommand;