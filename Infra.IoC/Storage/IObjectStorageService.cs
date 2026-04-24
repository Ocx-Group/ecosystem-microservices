using Microsoft.AspNetCore.Http;

namespace Ecosystem.Infra.IoC.Storage;

public interface IObjectStorageService
{
    Task<ObjectStorageUploadResult> UploadAsync(
        IFormFile file,
        string folder,
        string? fileName,
        CancellationToken cancellationToken);
}
