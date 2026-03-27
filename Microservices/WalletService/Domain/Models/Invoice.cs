
namespace Ecosystem.WalletService.Domain.Models;

public partial class Invoice
{
    public long Id { get; set; }

    public int InvoiceNumber { get; set; }

    public int? PurchaseOrderId { get; set; }

    public int AffiliateId { get; set; }

    public decimal? TotalInvoice { get; set; }

    public decimal? TotalInvoiceBtc { get; set; }

    public decimal? TotalCommissionable { get; set; }

    public int? TotalPoints { get; set; }

    public bool? State { get; set; }

    public bool Status { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? CancellationDate { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Bank { get; set; }

    public string? ReceiptNumber { get; set; }

    public DateOnly? DepositDate { get; set; }

    public bool? Type { get; set; }

    public string? Reason { get; set; }

    public string InvoiceData { get; set; } = null!;

    public string? InvoiceAddress { get; set; }

    public string? ShippingAddress { get; set; }

    public string? SecretKey { get; set; }

    public string? BtcAddress { get; set; }

    public int Recurring { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long BrandId { get; set; }
    
    public bool IncludeInCommissionCalculation { get; set; }
    public virtual ICollection<BonusTransactionHistory> BonusTransactionHistories { get; } = new List<BonusTransactionHistory>();

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<Credit> Credits { get; } = new List<Credit>();

    public virtual ICollection<InvoicePack> InvoicePacks { get; } = new List<InvoicePack>();

    public virtual ICollection<InvoicesDetail> InvoicesDetails { get; } = new List<InvoicesDetail>();
}
