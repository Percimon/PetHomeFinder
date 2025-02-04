using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.Volunteers.Commands.UpdatePetMainPhoto;

public record UpdatePetMainPhotoCommand(Guid VolunteerId, Guid PetId, string FilePath) : ICommand;