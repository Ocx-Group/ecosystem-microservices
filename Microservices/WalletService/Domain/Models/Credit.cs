
namespace Ecosystem.WalletService.Domain.Models;

public partial class Credit
{
    public long Id { get; set; }

    public int AffiliateId { get; set; }

    public long? InvoiceId { get; set; }

    public int? ConceptId { get; set; }

    public string? Concept { get; set; }

    public decimal? Credit1 { get; set; }

    public decimal? Debit { get; set; }

    public short? Paid { get; set; }

    public short Request { get; set; }

    public short? RequestDenied { get; set; }

    public decimal? Iva { get; set; }

    public decimal? Islr { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Invoice? Invoice { get; set; }
}
