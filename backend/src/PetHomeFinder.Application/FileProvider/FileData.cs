using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.FileProvider;

public record FileData(
    Stream FileStream,
    FilePath FilePath,
    string BucketName);