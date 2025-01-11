using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.Volunteers.Commands.FileTest.Upload;

public record UploadFileCommand(
    Stream FileStream,
    string FilePath,
    string BucketName) : ICommand;