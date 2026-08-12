using System.ComponentModel.DataAnnotations;

namespace Ecosystem.ConfigurationService.Application.DTOs;

/// <summary>
/// The purchase bonus settings a brand administrator manages from the commissions
/// section of their own dashboard. Backed by the commission columns of
/// <c>brand_configuration</c>, which WalletService already consumes over gRPC.
/// </summary>
public sealed record CommissionSettingsDto
{
    public long BrandId { get; init; }
    public bool CommissionEnabled { get; init; }

    /// <summary>Percentage paid to each upline level, index 0 being level 1.</summary>
    public decimal[] CommissionLevels { get; init; } = [];

    public bool DailyBonusAlwaysDistribute { get; init; }
    public DateTime UpdatedAt { get; init; }
}

public sealed record UpdateCommissionSettingsRequest
{
    public bool CommissionEnabled { get; init; }

    /// <summary>
    /// One percentage per upline level. Bounds are enforced here so an obviously
    /// malformed payload is rejected before the handler runs; the cross-field rules
    /// (total, emptiness while enabled) live in the handler because they need the
    /// whole request.
    /// </summary>
    [Required]
    [MaxLength(CommissionSettingsLimits.MaxLevels)]
    public decimal[] CommissionLevels { get; init; } = [];

    public bool DailyBonusAlwaysDistribute { get; init; }
}

public static class CommissionSettingsLimits
{
    public const int MaxLevels = 10;
    public const decimal MaxLevelPercentage = 100m;
    public const decimal MaxTotalPercentage = 100m;
}
