using PetHomeFinder.Core.Shared;

namespace PetHomeFinder.Core.Files;

public record FileInfo(
    FilePath FilePath,
    string BucketName);