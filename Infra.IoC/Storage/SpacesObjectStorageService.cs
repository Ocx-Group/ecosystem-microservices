using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Ecosystem.Domain.Core.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Ecosystem.Infra.IoC.Storage;

public class SpacesObjectStorageService : IObjectStorageService
{
    private readonly ObjectStorageSettings _settings;
    private readonly IAmazonS3 _client;

    public SpacesObjectStorageService(IOptions<ObjectStorageSettings> options)
    {
        _settings = options.Value;

        if (string.IsNullOrWhiteSpace(_settings.AccessKey) ||
            string.IsNullOrWhiteSpace(_settings.SecretKey) ||
            string.IsNullOrWhiteSpace(_settings.BucketName) ||
            string.IsNullOrWhiteSpace(_settings.PublicBaseUrl))
        {
            throw new InvalidOperationException("ObjectStorage configuration is incomplete.");
        }

        var credentials = new BasicAWSCredentials(_settings.AccessKey, _settings.SecretKey);
        var config = new AmazonS3Config
        {
            ServiceURL = $"https://{_settings.Region}.digitaloceanspaces.com",
            AuthenticationRegion = _settings.Region,
            ForcePathStyle = false
        };

        _client = new AmazonS3Client(credentials, config);
    }

    public async Task<ObjectStorageUploadResult> UploadAsync(
        IFormFile file,
        string folder,
        string? fileName,
        CancellationToken cancellationToken)
    {
        if (file.Length == 0) throw new ArgumentException("File is empty.", nameof(file));

        var key = BuildKey(folder, fileName ?? file.FileName);
        await using var stream = file.OpenReadStream();

        var request = new PutObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = key,
            InputStream = stream,
            ContentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
            CannedACL = S3CannedACL.PublicRead
        };

        await _client.PutObjectAsync(request, cancellationToken);

        return new ObjectStorageUploadResult(
            key,
            ObjectStorageUrl.BuildPublicUrl(_settings.PublicBaseUrl, key));
    }

    private static string BuildKey(string folder, string fileName)
    {
        var cleanFolder = SanitizePath(folder);
        var cleanFileName = Path.GetFileName(fileName.Replace('\\', '/'));
        return $"{cleanFolder.TrimEnd('/')}/{cleanFileName}";
    }

    private static string SanitizePath(string path)
        => path.Replace('\\', '/').Trim('/').Replace("..", string.Empty);
}
