using CSharpFunctionalExtensions;

namespace PetHomeFinder.Domain.Shared;

public class FilePath
{
    private FilePath(string path)
    {
        Path = path;
    }

    public string Path { get; }

    public static Result<FilePath, Error> Create(Guid path, string extension)
    {
        var fullPath = path + extension;

        return new FilePath(fullPath);
    }
    
    public static Result<FilePath, Error> Create(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return Errors.General.ValueIsInvalid("file path");

        return new FilePath(filePath);
    }
}