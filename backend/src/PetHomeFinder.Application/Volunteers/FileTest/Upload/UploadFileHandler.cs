using CSharpFunctionalExtensions;
using PetHomeFinder.Application.FileProvider;
using PetHomeFinder.Application.Providers;
using PetHomeFinder.Domain.Shared;
using FileInfo = PetHomeFinder.Application.FileProvider.FileInfo;

namespace PetHomeFinder.Application.Volunteers.FileTest.Upload;

public class UploadFileHandler
{
    private readonly IFileProvider _fileProvider;

    public UploadFileHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, ErrorList>> Handle(
        UploadFileRequest request,
        CancellationToken cancellationToken = default)
    {
        var filePathResult = FilePath.Create(request.FilePath);

        if (filePathResult.IsFailure)
            return filePathResult.Error.ToErrorList();
        
        var fileInfo = new FileInfo(
            filePathResult.Value,
            request.BucketName);

        var fileData = new FileData(
            request.FileStream,
            fileInfo);

        var result = await _fileProvider.UploadFile(fileData, cancellationToken);

        return result.Value;
    }
}