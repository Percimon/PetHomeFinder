using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.Volunteers.Application.Commands.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;
