using PetHomeFinder.Volunteers.Application.Commands.UpdatePetMainPhoto;

namespace PetHomeFinder.Volunteers.Contracts.Requests;

public record UpdatePetMainPhotoRequest(string FilePath)
{
    public UpdatePetMainPhotoCommand ToCommand(Guid VolunteerId, Guid PetId) =>
        new(VolunteerId, PetId, FilePath);
}