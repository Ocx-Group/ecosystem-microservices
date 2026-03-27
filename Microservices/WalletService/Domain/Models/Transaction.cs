
namespace Ecosystem.WalletService.Domain.Models;

public partial class Transaction
{
    public long Id { get; set; }

    public string IdTransaction { get; set; } = null!;

    public int AffiliateId { get; set; }

    public decimal Amount { get; set; }

    public decimal AmountReceived { get; set; }

    public string Products { get; set; } = null!;

    public bool Acredited { get; set; }

    public int Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Reference { get; set; }

    public long BrandId { get; set; }

    public virtual Brand Brand { get; set; } = null!;
    public string? Address { get; set; }
}
