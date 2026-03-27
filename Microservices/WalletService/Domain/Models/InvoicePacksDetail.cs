
namespace Ecosystem.WalletService.Domain.Models;

public partial class InvoicePacksDetail
{
    public long Id { get; set; }

    public long InvoicePackId { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual InvoicePack InvoicePack { get; set; } = null!;
}
