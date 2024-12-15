namespace PetHomeFinder.Application.Volunteers.Commands.FileTest.Delete;

public record DeleteFileRequest(
    string BucketName,
    string ObjectName);