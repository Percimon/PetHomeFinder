using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.FileProvider;
using PetHomeFinder.Application.Messaging;
using PetHomeFinder.Application.Providers;
using FileInfo = PetHomeFinder.Application.FileProvider.FileInfo;

namespace PetHomeFinder.Infrastructure.BackgroundServices;

public class FileCleanerBackgroundService : BackgroundService
{
    private readonly ILogger<FileCleanerBackgroundService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public FileCleanerBackgroundService(
        ILogger<FileCleanerBackgroundService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Files cleaner background service started.");

        await using var scope = _scopeFactory.CreateAsyncScope();

        var fileCleanerService = scope.ServiceProvider.GetRequiredService<IFileCleanerService>();

        while (!cancellationToken.IsCancellationRequested)
        {
            await fileCleanerService.Process(cancellationToken);
        }

        await Task.CompletedTask;
    }
}