namespace PetHomeFinder.Application.FileProvider;

public record FileData(
    Stream FileStream,
    string BucketName,
    string FilePath);