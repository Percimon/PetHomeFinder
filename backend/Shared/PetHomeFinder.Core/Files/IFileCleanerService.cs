namespace PetHomeFinder.Core.Files;

public interface IFileCleanerService
{
    Task Process(CancellationToken cancellationToken);
}