namespace PetHomeFinder.Application.Volunteers.FileTest.Upload;

public record UploadFileRequest(
    Stream FileStream,
    string FilePath,
    string BucketName);