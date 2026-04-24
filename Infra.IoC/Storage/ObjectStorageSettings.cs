namespace Ecosystem.Infra.IoC.Storage;

public class ObjectStorageSettings
{
    public const string SectionName = "ObjectStorage";

    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string Region { get; set; } = "nyc3";
    public string PublicBaseUrl { get; set; } = string.Empty;
}
