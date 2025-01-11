using CSharpFunctionalExtensions;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.FileProvider;
using PetHomeFinder.Application.Providers;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.Commands.FileTest.Get;

public class GetFileHandler : ICommandHandler<string, GetFileCommand>
{
    private readonly IFileProvider _fileProvider;

    public GetFileHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, ErrorList>> Handle(
        GetFileCommand command, 
        CancellationToken cancellationToken)
    {
        var fileMetaData = new FileMetaData(command.BucketName, command.ObjectName);
        var result = await _fileProvider.GetFile(fileMetaData, cancellationToken);

        return result;
    }
}