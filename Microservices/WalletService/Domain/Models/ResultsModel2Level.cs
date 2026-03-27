
namespace Ecosystem.WalletService.Domain.Models;

public partial class ResultsModel2Level
{
    public long Id { get; set; }

    public long ResultEcopoolId { get; set; }

    public int AffiliateId { get; set; }

    public string AffiliateName { get; set; } = null!;

    public int Level { get; set; }

    public decimal PercentageLevel { get; set; }

    public decimal CompanyPercentageLevel { get; set; }

    public decimal CompanyAmountLevel { get; set; }

    public decimal CommisionAmount { get; set; }

    public decimal PaymentAmount { get; set; }

    public decimal? Points { get; set; }

    public DateTime? CompletedAt { get; set; }

    public int BinarySide { get; set; }

    public DateTime UserCreatedAt { get; set; }

    public virtual ResultsModel2 ResultEcopool { get; set; } = null!;
}
