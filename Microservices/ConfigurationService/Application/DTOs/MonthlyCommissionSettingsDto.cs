using System.ComponentModel.DataAnnotations;

namespace Ecosystem.ConfigurationService.Application.DTOs;

/// <summary>
/// The parameters a brand administrator manages from admin/calculate-commissions to
/// drive the monthly liquidation. Backed by the <c>monthly_commission_*</c> columns of
/// <c>brand_configuration</c>, which WalletService reads over gRPC before calling
/// <c>calculate_monthly_commissions</c>.
///
/// Unrelated to <see cref="CommissionSettingsDto"/>: that one is the per-purchase
/// upline bonus, this one is the periodic payout on invoice balances.
/// </summary>
public sealed record MonthlyCommissionSettingsDto
{
    public long BrandId { get; init; }

    /// <summary>While false WalletService refuses to liquidate this brand.</summary>
    public bool Enabled { get; init; }

    /// <summary>Percentage of the invoice total paid for a full period.</summary>
    public decimal InterestRate { get; init; }

    /// <summary>Days an invoice created inside the period waits before earning.</summary>
    public int WaitingDays { get; init; }

    /// <summary>Payment group of the product this brand liquidates.</summary>
    public int? PaymentGroupId { get; init; }

    public DateTime UpdatedAt { get; init; }
}

public sealed record UpdateMonthlyCommissionSettingsRequest
{
    public bool Enabled { get; init; }

    /// <summary>
    /// Bounds are declared here so an obviously malformed payload is rejected before
    /// the handler runs. The rule that ties the payment group to
    /// <see cref="Enabled"/> lives in the handler because it spans two fields.
    /// </summary>
    [Range(0, (double)MonthlyCommissionSettingsLimits.MaxInterestRate)]
    public decimal InterestRate { get; init; }

    [Range(0, MonthlyCommissionSettingsLimits.MaxWaitingDays)]
    public int WaitingDays { get; init; }

    public int? PaymentGroupId { get; init; }
}

public static class MonthlyCommissionSettingsLimits
{
    /// <summary>
    /// The rate is a whole-period percentage of the invoice total, so anything at or
    /// near 100 already pays back the invoice every month. The cap only rules out the
    /// values that are certainly a typo.
    /// </summary>
    public const decimal MaxInterestRate = 100m;

    /// <summary>
    /// A wait longer than the period itself would pay nothing at all, and the longest
    /// period the endpoint accepts is a calendar month.
    /// </summary>
    public const int MaxWaitingDays = 90;
}
