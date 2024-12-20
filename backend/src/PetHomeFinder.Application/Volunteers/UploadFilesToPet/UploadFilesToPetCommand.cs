using PetHomeFinder.Application.DTOs;

namespace PetHomeFinder.Application.Volunteers.UploadFilesToPet;

public record UploadFilesToPetCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<UploadFileDto> Files);