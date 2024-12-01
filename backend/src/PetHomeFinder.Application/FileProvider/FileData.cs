namespace PetHomeFinder.Application.FileProvider;

public record FileData(
    Stream FileStream,
    FileInfo FileInfo);