using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.FileProvider;

public record FileInfo(
    FilePath FilePath,
    string BucketName);