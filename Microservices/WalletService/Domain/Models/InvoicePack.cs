
namespace Ecosystem.WalletService.Domain.Models;

public partial class InvoicePack
{
    public long Id { get; set; }

    public long InvoiceId { get; set; }

    public int ProductId { get; set; }

    public decimal BaseAmount { get; set; }

    public decimal Percentage { get; set; }

    public int CountDays { get; set; }

    public DateOnly StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;

    public virtual ICollection<InvoicePacksDetail> InvoicePacksDetails { get; } = new List<InvoicePacksDetail>();
}
