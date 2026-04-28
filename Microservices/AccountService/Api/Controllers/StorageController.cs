using Asp.Versioning;
using Ecosystem.AccountService.Api.Models;
using Ecosystem.Infra.IoC.Storage;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.AccountService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class StorageController : BaseController
{
    private readonly IObjectStorageService _storage;

    public StorageController(IObjectStorageService storage)
        => _storage = storage;

    [HttpPost("upload")]
    [RequestSizeLimit(10_485_760)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(
        [FromForm] UploadFileRequest request,
        CancellationToken ct)
    {
        var result = await _storage.UploadAsync(request.File, request.Folder, request.FileName, ct);
        return Ok(Success(result));
    }
}
