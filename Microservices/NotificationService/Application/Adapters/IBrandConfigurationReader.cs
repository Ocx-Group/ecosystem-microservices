namespace Ecosystem.NotificationService.Application.Adapters;

/// <summary>
/// Read-only notification and document identity obtained from the central
/// ConfigurationService. It intentionally excludes private business policies.
/// </summary>
public interface IBrandConfigurationReader
{
    Task<NotificationBrandConfiguration?> GetByBrandIdAsync(
        long brandId,
        CancellationToken cancellationToken = default);
}

public sealed record NotificationBrandConfiguration
{
    public long BrandId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string SenderName { get; init; } = string.Empty;
    public string SenderEmail { get; init; } = string.Empty;
    public string ClientUrl { get; init; } = string.Empty;
    public string CompanyName { get; init; } = string.Empty;
    public string SupportEmail { get; init; } = string.Empty;
    public string? CompanyIdentifier { get; init; }
    public string? SupportPhone { get; init; }
    public string? DocumentType { get; init; }
    public string? LogoUrl { get; init; }
    public string PrimaryColor { get; init; } = "#000000";
    public string SecondaryColor { get; init; } = "#FFFFFF";
    public string BackgroundColor { get; init; } = "#FFFFFF";
}
