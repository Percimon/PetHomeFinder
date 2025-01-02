using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.Volunteers.Commands.FileTest.Get;

public record GetFileCommand(
    string BucketName,
    string ObjectName) : ICommand;