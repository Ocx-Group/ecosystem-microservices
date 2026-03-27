
namespace Ecosystem.WalletService.Domain.Models;

public partial class VolumePurchase
{
    public long Id { get; set; }

    public int InvoiceId { get; set; }

    public int AffiliateId { get; set; }

    public int AffiliateIdGeneric { get; set; }

    public decimal? CreditI { get; set; }

    public decimal? CreditD { get; set; }

    public decimal? DebitI { get; set; }

    public decimal? DebitD { get; set; }

    public string? Concept { get; set; }

    public short? Compression { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
