using System.ComponentModel.DataAnnotations;

namespace Ecosystem.ConfigurationService.Application.DTOs;

public sealed record BrandingAdministrationDto
{
    public long BrandId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string CompanyName { get; init; } = string.Empty;
    public string? CompanyIdentifier { get; init; }
    public string ClientUrl { get; init; } = string.Empty;
    public string SupportEmail { get; init; } = string.Empty;
    public string? SupportPhone { get; init; }
    public string? DocumentType { get; init; }
    public string? LogoUrl { get; init; }
    public string PrimaryColor { get; init; } = "#000000";
    public string SecondaryColor { get; init; } = "#FFFFFF";
    public string BackgroundColor { get; init; } = "#FFFFFF";
    public DateTime UpdatedAt { get; init; }
}

public sealed record UpdateOwnBrandingRequest
{
    private const string HexColorPattern = "^#[0-9a-fA-F]{6}$";

    [Required, StringLength(150, MinimumLength = 2)]
    public string Name { get; init; } = string.Empty;

    [Required, StringLength(200, MinimumLength = 2)]
    public string CompanyName { get; init; } = string.Empty;

    [StringLength(100)]
    public string? CompanyIdentifier { get; init; }

    [Required, Url, StringLength(500)]
    public string ClientUrl { get; init; } = string.Empty;

    [Required, EmailAddress, StringLength(254)]
    public string SupportEmail { get; init; } = string.Empty;

    [Phone, StringLength(50)]
    public string? SupportPhone { get; init; }

    [StringLength(50)]
    public string? DocumentType { get; init; }

    [Url, StringLength(1000)]
    public string? LogoUrl { get; init; }

    [Required, RegularExpression(HexColorPattern)]
    public string PrimaryColor { get; init; } = "#000000";

    [Required, RegularExpression(HexColorPattern)]
    public string SecondaryColor { get; init; } = "#FFFFFF";

    [Required, RegularExpression(HexColorPattern)]
    public string BackgroundColor { get; init; } = "#FFFFFF";
}
