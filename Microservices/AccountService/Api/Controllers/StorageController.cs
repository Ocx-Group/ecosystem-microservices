using Asp.Versioning;
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
    public async Task<IActionResult> Upload(
        [FromForm] IFormFile file,
        [FromForm] string folder,
        [FromForm] string? fileName,
        CancellationToken ct)
    {
        var result = await _storage.UploadAsync(file, folder, fileName, ct);
        return Ok(Success(result));
    }
}
