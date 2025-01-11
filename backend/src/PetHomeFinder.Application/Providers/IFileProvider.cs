using CSharpFunctionalExtensions;
using PetHomeFinder.Application.FileProvider;
using PetHomeFinder.Domain.Shared;
using FileInfo = PetHomeFinder.Application.FileProvider.FileInfo;

namespace PetHomeFinder.Application.Providers;

public interface IFileProvider
{
    Task<Result<string, ErrorList>> UploadFile(
        FileData fileData, 
        CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<FilePath>, ErrorList>> UploadFiles(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default);
    
    Task<Result<string, ErrorList>> DeleteFile(
        FileMetaData fileMetaData, 
        CancellationToken cancellationToken = default);
    
    Task<UnitResult<ErrorList>> DeleteFile(
        FileInfo fileInfo, 
        CancellationToken cancellationToken = default);
    
    Task<Result<string, ErrorList>> GetFile(
        FileMetaData fileMetaData, 
        CancellationToken cancellationToken = default);
}