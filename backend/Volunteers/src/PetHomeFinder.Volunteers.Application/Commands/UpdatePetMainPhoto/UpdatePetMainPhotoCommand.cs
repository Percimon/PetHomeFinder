using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.Volunteers.Application.Commands.UpdatePetMainPhoto;

public record UpdatePetMainPhotoCommand(Guid VolunteerId, Guid PetId, string FilePath) : ICommand;