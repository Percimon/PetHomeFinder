using PetHomeFinder.Application.Volunteers.Commands.UpdatePetMainPhoto;

namespace PetHomeFinder.API.Controllers.Volunteers.Requests;

public record UpdatePetMainPhotoRequest(string FilePath)
{
    public UpdatePetMainPhotoCommand ToCommand(Guid VolunteerId, Guid PetId) =>
        new(VolunteerId, PetId, FilePath);
}