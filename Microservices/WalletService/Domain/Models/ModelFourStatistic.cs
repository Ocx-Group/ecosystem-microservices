
namespace Ecosystem.WalletService.Domain.Models;

public partial class ModelFourStatistic
{
    public long Id { get; set; }

    public int AffiliateId { get; set; }

    public int AffiliateNetworkId { get; set; }

    public decimal? CreditLeft { get; set; }

    public decimal? CreditRight { get; set; }

    public decimal? DebitLeft { get; set; }

    public decimal? DebitRight { get; set; }

    public int InvoiceId { get; set; }

    public string? Concept { get; set; }

    public DateTime Date { get; set; }

    public bool Compression { get; set; }
}
