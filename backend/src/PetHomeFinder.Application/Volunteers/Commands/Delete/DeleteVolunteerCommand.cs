using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.Volunteers.Commands.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;
