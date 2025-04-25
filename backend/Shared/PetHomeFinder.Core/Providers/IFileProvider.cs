using CSharpFunctionalExtensions;
using PetHomeFinder.Core.Files;
using PetHomeFinder.Core.Shared;
using PetHomeFinder.SharedKernel;
using FileInfo = PetHomeFinder.Core.Files.FileInfo;

namespace PetHomeFinder.Core.Providers;

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