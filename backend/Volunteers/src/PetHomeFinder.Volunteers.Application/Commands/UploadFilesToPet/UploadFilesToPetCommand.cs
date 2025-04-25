using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Dtos;

namespace PetHomeFinder.Volunteers.Application.Commands.UploadFilesToPet;

public record UploadFilesToPetCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<UploadFileDto> Files) : ICommand;