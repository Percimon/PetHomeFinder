namespace PetHomeFinder.Application.Volunteers.Commands.FileTest.Get;

public record GetFileRequest(
    string BucketName,
    string ObjectName);