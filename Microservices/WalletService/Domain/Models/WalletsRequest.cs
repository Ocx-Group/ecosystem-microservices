
namespace Ecosystem.WalletService.Domain.Models;

public partial class WalletsRequest
{
    public int AffiliateId { get; set; }

    public int? PaymentAffiliateId { get; set; }

    public string OrderNumber { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Concept { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public short Status { get; set; }

    public DateTime? AttentionDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long Id { get; set; }

    public string? AdminUserName { get; set; }

    public string Type { get; set; } = null!;

    public long InvoiceNumber { get; set; }

    public long? BrandId { get; set; }

    public virtual Brand? Brand { get; set; }
}
