using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.FileProvider;
using PetHomeFinder.Application.Messaging;
using PetHomeFinder.Application.Providers;
using PetHomeFinder.Infrastructure.BackgroundServices;
using FileInfo = PetHomeFinder.Application.FileProvider.FileInfo;

namespace PetHomeFinder.Infrastructure.Files;

public class FileCleanerService : IFileCleanerService
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<FileCleanerBackgroundService> _logger;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;

    public FileCleanerService(
        IFileProvider fileProvider,
        ILogger<FileCleanerBackgroundService> logger,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue)
    {
        _fileProvider = fileProvider;
        _logger = logger;
        _messageQueue = messageQueue;
    }

    public async Task Process(CancellationToken cancellationToken)
    {
        var fileInfos = await _messageQueue.ReadAsync(cancellationToken);

        foreach (var fileInfo in fileInfos)
        {
            await _fileProvider.DeleteFile(fileInfo, cancellationToken);
        }
    }
}