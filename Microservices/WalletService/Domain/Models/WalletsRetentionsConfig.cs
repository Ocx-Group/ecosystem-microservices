
namespace Ecosystem.WalletService.Domain.Models;

public partial class WalletsRetentionsConfig
{
    public decimal WithdrawalFrom { get; set; }

    public decimal WithdrawalTo { get; set; }

    public decimal Percentage { get; set; }

    public DateTime Date { get; set; }

    public DateTime? DisableDate { get; set; }

    public bool Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long Id { get; set; }
}
