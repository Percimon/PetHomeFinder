using CSharpFunctionalExtensions;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.FileProvider;
using PetHomeFinder.Application.Providers;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.Commands.FileTest.Delete;

public class DeleteFileHandler : ICommandHandler<string, DeleteFileCommand>
{
    private readonly IFileProvider _fileProvider;

    public DeleteFileHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, ErrorList>> Handle(
        DeleteFileCommand command, 
        CancellationToken cancellationToken)
    {
        var fileMetaData = new FileMetaData(command.BucketName, command.ObjectName);
        var result = await _fileProvider.DeleteFile(fileMetaData, cancellationToken);

        return result;
    }
}