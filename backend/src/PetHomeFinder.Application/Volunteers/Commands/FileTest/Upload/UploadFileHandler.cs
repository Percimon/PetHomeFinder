using CSharpFunctionalExtensions;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.FileProvider;
using PetHomeFinder.Application.Providers;
using PetHomeFinder.Domain.Shared;
using FileInfo = PetHomeFinder.Application.FileProvider.FileInfo;

namespace PetHomeFinder.Application.Volunteers.Commands.FileTest.Upload;

public class UploadFileHandler : ICommandHandler<string, UploadFileCommand>
{
    private readonly IFileProvider _fileProvider;

    public UploadFileHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, ErrorList>> Handle(
        UploadFileCommand command,
        CancellationToken cancellationToken = default)
    {
        var filePathResult = FilePath.Create(command.FilePath);

        if (filePathResult.IsFailure)
            return filePathResult.Error.ToErrorList();
        
        var fileInfo = new FileInfo(
            filePathResult.Value,
            command.BucketName);

        var fileData = new FileData(
            command.FileStream,
            fileInfo);

        var result = await _fileProvider.UploadFile(fileData, cancellationToken);

        return result.Value;
    }
}