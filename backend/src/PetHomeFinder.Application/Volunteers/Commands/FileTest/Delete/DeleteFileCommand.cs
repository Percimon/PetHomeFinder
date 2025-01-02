using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.Volunteers.Commands.FileTest.Delete;

public record DeleteFileCommand(
    string BucketName,
    string ObjectName) : ICommand;