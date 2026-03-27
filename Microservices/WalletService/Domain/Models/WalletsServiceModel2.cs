
namespace Ecosystem.WalletService.Domain.Models;

public partial class WalletsServiceModel2
{
    public long Id { get; set; }

    public int AffiliateId { get; set; }

    public int? UserId { get; set; }

    public decimal? Credit { get; set; }

    public decimal? Debit { get; set; }

    public bool? Status { get; set; }

    public string Concept { get; set; } = null!;

    public string? AffiliateUserName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? BrandId { get; set; }

    public virtual Brand? Brand { get; set; }
}
