namespace Ecosystem.Domain.Core.Storage;

public static class ObjectStorageUrl
{
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;

        var publicBaseUrl = Environment.GetEnvironmentVariable("ObjectStorage__PublicBaseUrl");
        if (string.IsNullOrWhiteSpace(publicBaseUrl)) return value;

        var key = ExtractKey(value);
        return string.IsNullOrWhiteSpace(key) ? value : BuildPublicUrl(publicBaseUrl, key);
    }

    public static string? ExtractKey(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;

        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
            return CleanKey(value);

        if (uri.Host.Equals("firebasestorage.googleapis.com", StringComparison.OrdinalIgnoreCase))
        {
            const string marker = "/o/";
            var markerIndex = uri.AbsolutePath.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
            if (markerIndex >= 0)
            {
                var encodedKey = uri.AbsolutePath[(markerIndex + marker.Length)..];
                return CleanKey(Uri.UnescapeDataString(encodedKey));
            }
        }

        if (uri.Host.Contains("digitaloceanspaces.com", StringComparison.OrdinalIgnoreCase))
            return CleanKey(uri.AbsolutePath.TrimStart('/'));

        return value;
    }

    public static string BuildPublicUrl(string publicBaseUrl, string key)
        => $"{publicBaseUrl.TrimEnd('/')}/{CleanKey(key)}";

    private static string CleanKey(string key)
        => key.Replace('\\', '/').TrimStart('/');
}
