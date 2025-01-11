using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.DTOs;

namespace PetHomeFinder.Application.Volunteers.Commands.UploadFilesToPet;

public record UploadFilesToPetCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<UploadFileDto> Files) : ICommand;