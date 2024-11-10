using CSharpFunctionalExtensions;
using PetHomeFinder.Application.FileProvider;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Providers;

public interface IFileProvider
{
    Task<Result<string, Error>> UploadFile(FileData fileData, CancellationToken cancellationToken = default);
    
    Task<Result<string, Error>> DeleteFile(FileMetaData fileMetaData, CancellationToken cancellationToken = default);
    
    Task<Result<string, Error>> GetFile(FileMetaData fileMetaData, CancellationToken cancellationToken = default);
}