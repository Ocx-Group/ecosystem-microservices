
namespace Ecosystem.WalletService.Domain.Models;

public partial class NetworkPurchase
{
    public long Id { get; set; }

    public int InvoiceId { get; set; }

    public int AffiliateId { get; set; }

    public int Level { get; set; }

    public decimal CommisionableAmount { get; set; }

    public decimal? Points { get; set; }

    public short Origin { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
