namespace PetHomeFinder.Application.FileProvider;

public interface IFileCleanerService
{
    Task Process(CancellationToken cancellationToken);
}