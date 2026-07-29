namespace Ecosystem.ConfigurationService.Application.DTOs;

/// <summary>
/// Public, read-only subset of brand configuration used to bootstrap a web
/// client before the user authenticates.
/// </summary>
public sealed record PublicBrandingDto
{
    public long BrandId { get; init; }
    public string ClientId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string CompanyName { get; init; } = string.Empty;
    public string ClientUrl { get; init; } = string.Empty;
    public string SupportEmail { get; init; } = string.Empty;
    public string? SupportPhone { get; init; }
    public string? DocumentType { get; init; }
    public string? LogoUrl { get; init; }
    public string PrimaryColor { get; init; } = "#000000";
    public string SecondaryColor { get; init; } = "#FFFFFF";
    public string BackgroundColor { get; init; } = "#FFFFFF";
}
