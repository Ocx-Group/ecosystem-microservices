namespace Ecosystem.ConfigurationService.Domain.Models;

/// <summary>
/// Dynamic brand configuration entity.
/// Each brand/website configures these settings from its own dashboard.
/// Replaces all hardcoded BrandId switch statements across microservices.
/// </summary>
public class BrandConfiguration
{
    public long Id { get; set; }
    public long BrandId { get; set; }

    // Admin
    public string AdminUserName { get; set; } = null!;

    // Email / notifications
    public string SenderName { get; set; } = null!;
    public string SenderEmail { get; set; } = null!;
    public string EmailTemplateFolder { get; set; } = null!;

    // Frontend
    public string ClientUrl { get; set; } = null!;

    // Commission distribution
    public bool CommissionEnabled { get; set; }
    public string CommissionLevelsJson { get; set; } = "[]";
    public decimal BonusPercentage { get; set; }

    // PDF / Invoice branding
    public string PdfTemplateName { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public string? CompanyIdentifier { get; set; }
    public string SupportEmail { get; set; } = null!;
    public string? SupportPhone { get; set; }
    public string? DocumentType { get; set; }
    public string? LogoUrl { get; set; }

    // Theme colors
    public string PrimaryColor { get; set; } = "#000000";
    public string SecondaryColor { get; set; } = "#FFFFFF";
    public string BackgroundColor { get; set; } = "#FFFFFF";

    // Affiliate tree
    public int? DefaultFatherAffiliateId { get; set; }
    public bool ActivateOnRegistration { get; set; } = true;

    // Payment groups
    public int? DefaultPaymentGroupId { get; set; }
    public int? TradingAcademyPaymentGroupId { get; set; }

    // Withdrawal rules
    public string WithdrawalValidationType { get; set; } = "None";
    public string? WithdrawalTimeZone { get; set; }
    public int? WithdrawalStartHour { get; set; }
    public int? WithdrawalEndHour { get; set; }
    public decimal? WithdrawalCapNoDirects { get; set; }
    public bool Requires10PercentPurchaseRule { get; set; }
    public bool PoolValidationRequired { get; set; }

    // Crypto / ConPayment
    public bool ConPaymentEnabled { get; set; }
    public string? ConPaymentAddress { get; set; }
    public int? BlockchainNetworkId { get; set; }

    // Status & audit
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navigation
    public Brand Brand { get; set; } = null!;
}
