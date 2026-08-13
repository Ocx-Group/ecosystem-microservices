namespace Ecosystem.Domain.Core.BrandConfiguration;

/// <summary>
/// Shared brand configuration DTO used by all microservices.
/// Loaded from ConfigurationService, cached aggressively since it rarely changes.
/// Each brand/website configures this from its own dashboard.
/// </summary>
public record BrandConfigurationDto
{
    public long BrandId { get; init; }
    public string Name { get; init; } = null!;
    public string AdminUserName { get; init; } = null!;

    // Email / notifications
    public string SenderName { get; init; } = null!;
    public string SenderEmail { get; init; } = null!;
    public string EmailTemplateFolder { get; init; } = null!;

    // Frontend
    public string ClientUrl { get; init; } = null!;

    // Commission distribution
    public bool CommissionEnabled { get; init; }
    public decimal[] CommissionLevels { get; init; } = [];
    public decimal BonusPercentage { get; init; }

    /// <summary>
    /// When false the purchase bonus is only distributed for purchases that opted in
    /// via <c>WalletRequest.DailyBonusActivation</c>.
    /// </summary>
    public bool DailyBonusAlwaysDistribute { get; init; }

    // Monthly commission liquidation

    /// <summary>
    /// While false WalletService refuses to run the monthly liquidation for the
    /// brand, so a misconfigured brand cannot be paid out by accident.
    /// </summary>
    public bool MonthlyCommissionEnabled { get; init; }

    /// <summary>Percentage of the invoice total paid for a full period.</summary>
    public decimal MonthlyCommissionInterestRate { get; init; }

    /// <summary>Days an invoice created inside the period waits before earning.</summary>
    public int MonthlyCommissionWaitingDays { get; init; }

    /// <summary>Payment group of the product this brand liquidates.</summary>
    public int? MonthlyCommissionPaymentGroupId { get; init; }

    // PDF / Invoice branding
    public string PdfTemplateName { get; init; } = null!;
    public string CompanyName { get; init; } = null!;
    public string? CompanyIdentifier { get; init; }
    public string SupportEmail { get; init; } = null!;
    public string? SupportPhone { get; init; }
    public string? DocumentType { get; init; }
    public string? LogoUrl { get; init; }

    // Theme colors
    public string PrimaryColor { get; init; } = "#000000";
    public string SecondaryColor { get; init; } = "#FFFFFF";
    public string BackgroundColor { get; init; } = "#FFFFFF";

    // Affiliate tree
    public int? DefaultFatherAffiliateId { get; init; }
    public bool ActivateOnRegistration { get; init; } = true;

    // Payment groups
    public int? DefaultPaymentGroupId { get; init; }
    public int? TradingAcademyPaymentGroupId { get; init; }

    // Withdrawal rules
    public string WithdrawalValidationType { get; init; } = "None";
    public string? WithdrawalTimeZone { get; init; }
    public int? WithdrawalStartHour { get; init; }
    public int? WithdrawalEndHour { get; init; }
    public decimal? WithdrawalCapNoDirects { get; init; }
    public bool Requires10PercentPurchaseRule { get; init; }
    public bool PoolValidationRequired { get; init; }

    // Crypto / ConPayment
    public bool ConPaymentEnabled { get; init; }
    public string? ConPaymentAddress { get; init; }
    public int? BlockchainNetworkId { get; init; }

    public bool IsActive { get; init; } = true;
}
