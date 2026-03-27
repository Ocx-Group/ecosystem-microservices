
namespace Ecosystem.WalletService.Domain.Models;

public partial class WalletsWithdrawal
{
    public int AffiliateId { get; set; }

    public decimal Amount { get; set; }

    public bool Status { get; set; }

    public string? Observation { get; set; }

    public string? AdminObservation { get; set; }

    public DateTime Date { get; set; }

    public DateTime? ResponseDate { get; set; }

    public decimal RetentionPercentage { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long Id { get; set; }

    public bool IsProcessed { get; set; }

    public string? AffiliateUserName { get; set; }
}
