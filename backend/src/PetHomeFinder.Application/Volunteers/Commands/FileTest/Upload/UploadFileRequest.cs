namespace PetHomeFinder.Application.Volunteers.Commands.FileTest.Upload;

public record UploadFileRequest(
    Stream FileStream,
    string FilePath,
    string BucketName);