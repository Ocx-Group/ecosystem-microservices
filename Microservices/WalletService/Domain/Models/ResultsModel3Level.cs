
namespace Ecosystem.WalletService.Domain.Models;

public partial class ResultsModel3Level
{
    public long Id { get; set; }

    public long ResultsModel3Id { get; set; }

    public int AffiliateId { get; set; }

    public string AffiliateName { get; set; } = null!;

    public int Level { get; set; }

    public decimal PercentageLevel { get; set; }

    public decimal PaymentAmount { get; set; }

    public decimal? Points { get; set; }

    public DateTime? CompletedAt { get; set; }

    public int BinarySide { get; set; }

    public DateTime UserCreatedAt { get; set; }

    public virtual ResultsModel3 ResultsModel3 { get; set; } = null!;
}
