namespace PetHomeFinder.Application.Volunteers.FileTest.Get;

public record GetFileRequest(
    string BucketName,
    string ObjectName);