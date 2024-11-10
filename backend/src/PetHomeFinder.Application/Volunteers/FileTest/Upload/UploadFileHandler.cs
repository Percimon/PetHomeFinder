using CSharpFunctionalExtensions;
using PetHomeFinder.Application.FileProvider;
using PetHomeFinder.Application.Providers;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.FileTest.Upload;

public class UploadFileHandler
{
    private readonly IFileProvider _fileProvider;

    public UploadFileHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, Error>> Handle(
        UploadFileRequest request,
        CancellationToken cancellationToken = default)
    {
        var fileData = new FileData(
            request.FileStream,
            request.BucketName,
            request.FilePath);
        
        var result = await _fileProvider.UploadFile(fileData, cancellationToken);

        return result;
    }
}