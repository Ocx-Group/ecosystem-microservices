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

    /// <summary>
    /// When false (the default) the per-purchase bonus is only distributed if the
    /// purchase itself opted in via <c>WalletRequest.DailyBonusActivation</c>. When
    /// true every purchase of the brand distributes it.
    ///
    /// The flag is phrased so that its "off" state is the CLR default of
    /// <see cref="bool"/>. A property defaulting to <c>true</c> would collide with
    /// the store default on insert and silently ignore an operator turning it off.
    /// </summary>
    public bool DailyBonusAlwaysDistribute { get; set; }

    // Monthly commission liquidation
    //
    // Feeds calculate_monthly_commissions. Unrelated to the per-purchase bonus
    // above: this is the periodic payout run by an administrator from
    // admin/calculate-commissions, and every brand liquidates a different
    // product payment group at its own rate.

    /// <summary>
    /// While false the liquidation endpoint refuses to run for this brand. New
    /// brands start disabled so nobody can trigger a payout before the rate,
    /// the waiting days and the payment group have been reviewed.
    /// </summary>
    public bool MonthlyCommissionEnabled { get; set; }

    /// <summary>Percentage of the invoice total paid for a full period.</summary>
    public decimal MonthlyCommissionInterestRate { get; set; }

    /// <summary>Days an invoice created inside the period waits before earning.</summary>
    public int MonthlyCommissionWaitingDays { get; set; }

    /// <summary>
    /// The payment group of the product this brand liquidates. Nullable because a
    /// brand that never liquidates has none, and because the
    /// <see cref="MonthlyCommissionSources.InvoiceTotal"/> source ignores it; it is
    /// required to enable the feature under the payment-group source.
    /// </summary>
    public int? MonthlyCommissionPaymentGroupId { get; set; }

    /// <summary>
    /// Which stored procedure WalletService calls for this brand. The accepted values
    /// live in <c>Ecosystem.Domain.Core.BrandConfiguration.MonthlyCommissionSources</c>
    /// (<c>PaymentGroup</c> / <c>InvoiceTotal</c>); the literal is repeated here because
    /// this project deliberately has no reference to the shared kernel. Defaults to the
    /// payment-group variant, so an existing brand keeps behaving exactly as before.
    ///
    /// RecyBot needs <c>InvoiceTotal</c>: its invoices came over from RecyCoin through
    /// a data migration that wrote no <c>invoices_details</c> rows, and the
    /// payment-group procedure reaches invoices only through their details.
    /// </summary>
    public string MonthlyCommissionSource { get; set; } = "PaymentGroup";

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
