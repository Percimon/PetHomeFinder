using CSharpFunctionalExtensions;
using PetHomeFinder.Application.FileProvider;
using PetHomeFinder.Domain.Shared;
using FileInfo = PetHomeFinder.Application.FileProvider.FileInfo;

namespace PetHomeFinder.Application.Providers;

public interface IFileProvider
{
    Task<Result<string, Error>> UploadFile(
        FileData fileData, 
        CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default);
    
    Task<Result<string, Error>> DeleteFile(
        FileMetaData fileMetaData, 
        CancellationToken cancellationToken = default);
    
    Task<UnitResult<Error>> DeleteFile(
        FileInfo fileInfo, 
        CancellationToken cancellationToken = default);
    
    Task<Result<string, Error>> GetFile(
        FileMetaData fileMetaData, 
        CancellationToken cancellationToken = default);
}