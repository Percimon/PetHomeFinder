using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetHomeFinder.Application.FileProvider;
using PetHomeFinder.Application.Providers;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private const int MAX_DEGREE_OF_PARALLELISM = 10;
    private const int EXPIRY = 60 * 60 * 24;

    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(
        IMinioClient minioClient,
        ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<string, Error>> UploadFile(
        FileData fileData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await CreateBucketIfNotExists(fileData.BucketName, cancellationToken);
            
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithStreamData(fileData.FileStream)
                .WithObjectSize(fileData.FileStream.Length)
                .WithObject(fileData.FilePath.Path);

            var result = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
            return result.ObjectName;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to upload file to minio");
            return Error.Failure("file.upload", "Failed to upload file to minio");
        }
    }

    public async Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);
        var filesList = filesData.ToList();
        
        try
        {
            await CreateBucketsIfNotExist(
                filesList.Select(file => file.BucketName), 
                cancellationToken);

            var tasks = filesList.Select(
                async file => await PutObject(file, semaphoreSlim, cancellationToken));

            var pathResult = await Task.WhenAll(tasks);

            if (pathResult.Any(p => p.IsFailure))
                return pathResult.First().Error;

            var results = pathResult.Select(p => p.Value).ToList();

            _logger.LogInformation("Uploaded files {files}", results);

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload files in minio, files amount: {amount}", filesList.Count);

            return Error.Failure("file.upload", "Fail to upload files in minio");
        }
    }

    public async Task<Result<string, Error>> DeleteFile(
        FileMetaData fileMetaData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketExist = await IsBucketExist(fileMetaData.BucketName, cancellationToken);
            if (bucketExist == false)
            {
                return Error.Failure("file.delete", $"Bucket {fileMetaData.BucketName} not found");
            }

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(fileMetaData.BucketName)
                .WithObject(fileMetaData.ObjectName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

            _logger.LogInformation("Deleted file {objectName} from minio", fileMetaData.ObjectName);

            return fileMetaData.ObjectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete file from minio");
            return Error.Failure("file.delete", "Failed to delete file from minio");
        }
    }

    public async Task<Result<string, Error>> GetFile(
        FileMetaData fileMetaData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketExist = await IsBucketExist(fileMetaData.BucketName, cancellationToken);
            if (bucketExist == false)
            {
                return Error.Failure("file.get", $"Bucket {fileMetaData.BucketName} not found");
            }

            var statObjectArgs = new StatObjectArgs()
                .WithBucket(fileMetaData.BucketName)
                .WithObject(fileMetaData.ObjectName);
            var statObject = await _minioClient.StatObjectAsync(statObjectArgs, cancellationToken);
            if (statObject.Size == 0)
            {
                return Error.Failure("file.get", $"File {fileMetaData.ObjectName} not found");
            }
            
            return await GetFileUrl(fileMetaData);;
        }
        catch (Minio.Exceptions.ObjectNotFoundException)
        {
            return Error.Failure("file.get", $"File {fileMetaData.ObjectName} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get file from minio");
            return Error.Failure("file.get", "Failed to get file from minio");
        }
    }

    private async Task CreateBucketIfNotExists(
        string bucketName,
        CancellationToken cancellationToken = default)
    {
        var bucketExist = await IsBucketExist(bucketName, cancellationToken);
        
        if (bucketExist == false)
        {
            await CreateBucket(bucketName, cancellationToken);
        }
    }
    
    private async Task CreateBucketsIfNotExist(
        IEnumerable<string> buckets,
        CancellationToken cancellationToken = default)
    {
        HashSet<String> bucketNames = [..buckets];

        foreach (var bucketName in bucketNames)
        {
            await CreateBucketIfNotExists(bucketName, cancellationToken);
        }
    }
    
    private async Task<bool> IsBucketExist(
        string bucketName,
        CancellationToken cancellationToken = default)
    {
        var bucketExistArgs = new BucketExistsArgs().WithBucket(bucketName);
        var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);

        return bucketExist;
    }

    private async Task CreateBucket(
        string bucketName,
        CancellationToken cancellationToken = default)
    {
        var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);
        await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
    }

    private async Task<string> GetFileUrl(FileMetaData fileMetaData)
    {
        var presignedObjectArgs = new PresignedGetObjectArgs()
            .WithBucket(fileMetaData.BucketName)
            .WithObject(fileMetaData.ObjectName)
            .WithExpiry(EXPIRY);

        return await _minioClient.PresignedGetObjectAsync(presignedObjectArgs);
    }
    
    private async Task<Result<FilePath, Error>> PutObject(
        FileData fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken = default)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(fileData.BucketName)
            .WithStreamData(fileData.FileStream)
            .WithObjectSize(fileData.FileStream.Length)
            .WithObject(fileData.FilePath.Path);

        try
        {
            await _minioClient
                .PutObjectAsync(putObjectArgs, cancellationToken);

            return fileData.FilePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with path {path} in bucket {bucket}",
                fileData.FilePath.Path,
                fileData.BucketName);

            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }
}