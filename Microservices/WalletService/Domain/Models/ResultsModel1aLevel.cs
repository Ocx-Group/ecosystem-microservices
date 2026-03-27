
namespace Ecosystem.WalletService.Domain.Models;

public partial class ResultsModel1aLevel
{
    public long Id { get; set; }

    public long ResultsModel1aId { get; set; }

    public int AffiliateId { get; set; }

    public string AffiliateName { get; set; } = null!;

    public int Level { get; set; }

    public decimal PercentageLevel { get; set; }

    public decimal PaymentAmount { get; set; }

    public decimal? Points { get; set; }

    public DateTime? CompletedAt { get; set; }

    public int BinarySide { get; set; }

    public DateTime UserCreatedAt { get; set; }

    public virtual ResultsModel1a ResultsModel1a { get; set; } = null!;
}
