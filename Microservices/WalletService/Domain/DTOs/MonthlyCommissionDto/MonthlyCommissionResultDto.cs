namespace Ecosystem.WalletService.Domain.DTOs.MonthlyCommissionDto;

/// <summary>
/// Outcome of one liquidation run. The same shape is returned for a simulation and for
/// a real run, so the screen renders one table either way and only the
/// <see cref="DryRun"/> flag changes the wording.
/// </summary>
public sealed record MonthlyCommissionResultDto
{
    public bool DryRun { get; init; }

    /// <summary>Number of affiliates paid, or that would be paid on a simulation.</summary>
    public int RowsAffected { get; init; }

    public decimal TotalCredit { get; init; }

    /// <summary>
    /// The period the run covered, echoed back so the operator can tell at a glance
    /// which month the numbers belong to.
    /// </summary>
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }

    /// <summary>The parameters actually used, after brand defaults were applied.</summary>
    public decimal InterestRate { get; init; }
    public int WaitingDays { get; init; }
    public int PaymentGroupId { get; init; }

    public IReadOnlyList<MonthlyCommissionItemDto> Items { get; init; } = [];
}

public sealed record MonthlyCommissionItemDto
{
    public int AffiliateId { get; init; }
    public string AffiliateUserName { get; init; } = string.Empty;
    public decimal Credit { get; init; }
}
