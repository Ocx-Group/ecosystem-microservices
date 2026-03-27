
namespace Ecosystem.WalletService.Domain.Models;

public partial class WalletsWait
{
    public int AffiliateId { get; set; }

    public decimal? Credit { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string? Bank { get; set; }

    public string? Support { get; set; }

    public DateTime? DepositDate { get; set; }

    public bool? Status { get; set; }

    public bool? Attended { get; set; }

    public DateTime Date { get; set; }

    public string? Order { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long Id { get; set; }
}
