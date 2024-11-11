namespace PetHomeFinder.Application.Volunteers.FileTest.Delete;

public record DeleteFileRequest(
    string BucketName,
    string ObjectName);