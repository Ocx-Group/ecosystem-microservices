namespace Ecosystem.WalletService.Domain.Requests.WalletRequest;

/// <summary>
/// A single run of the monthly liquidation, as requested from
/// admin/calculate-commissions.
///
/// Note what is absent: the brand and the administrator name. Both are resolved
/// server-side from the authenticated tenant and its brand configuration, so a caller
/// cannot liquidate someone else's brand or forge the audit trail.
/// </summary>
public class CalculateMonthlyCommissionsRequest
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    /// <summary>
    /// Per-run override of the brand default. Null means "use what the brand has
    /// configured", which is the normal case.
    /// </summary>
    public decimal? InterestRate { get; set; }

    /// <summary>Per-run override of the brand default.</summary>
    public int? WaitingDays { get; set; }

    /// <summary>Per-run override of the brand default.</summary>
    public int? PaymentGroupId { get; set; }

    /// <summary>
    /// When true the function reports what it would pay without writing a single
    /// wallet row. This is what backs the "Simular" mode of the screen.
    /// </summary>
    public bool DryRun { get; set; }
}
