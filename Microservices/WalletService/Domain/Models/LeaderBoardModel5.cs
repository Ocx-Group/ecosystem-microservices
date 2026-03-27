
namespace Ecosystem.WalletService.Domain.Models;

public partial class LeaderBoardModel5
{
    public long Id { get; set; }

    public int AffiliateId { get; set; }

    public string MatrixPosition { get; set; } = null!;

    public int GradingPosition { get; set; }

    public DateTime CreatedAt { get; set; }

    public decimal Amount { get; set; }

    public string UserName { get; set; } = null!;

    public int GradingId { get; set; }
}
