using CSharpFunctionalExtensions;
using PetHomeFinder.Application.FileProvider;
using PetHomeFinder.Application.Providers;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.FileTest.Get;

public class GetFileHandler
{
    private readonly IFileProvider _fileProvider;

    public GetFileHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, Error>> Handle(
        GetFileRequest request, 
        CancellationToken cancellationToken)
    {
        var fileMetaData = new FileMetaData(request.BucketName, request.ObjectName);
        var result = await _fileProvider.GetFile(fileMetaData, cancellationToken);

        return result;
    }
}