using Microsoft.AspNetCore.Mvc;
using Minio;
using PetHomeFinder.API.Extensions;
using PetHomeFinder.Application.Volunteers.FileTest.Delete;
using PetHomeFinder.Application.Volunteers.FileTest.Get;
using PetHomeFinder.Application.Volunteers.FileTest.Upload;

namespace PetHomeFinder.API.Controllers;

public class FileController : ApplicationController
{
    public const string BUCKET_NAME_PHOTOS = "photos";

    private readonly IMinioClient _minioClient;

    public FileController(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(
        IFormFile file,
        [FromServices] UploadFileHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        await using var stream = file.OpenReadStream();

        var request = new UploadFileRequest(
            stream,
            BUCKET_NAME_PHOTOS,
            Guid.NewGuid().ToString());

        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{fileName:guid}")]
    public async Task<IActionResult> RemoveFile(
        [FromRoute] Guid fileName,
        [FromServices] DeleteFileHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteFileRequest(BUCKET_NAME_PHOTOS, fileName.ToString());
        
        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToResponse();
        }

        return Ok(result.Value);
    }

    [HttpGet("{fileName:guid}")]
    public async Task<ActionResult> Get(
        [FromRoute] Guid fileName,
        [FromServices] GetFileHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetFileRequest(BUCKET_NAME_PHOTOS, fileName.ToString());

        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}