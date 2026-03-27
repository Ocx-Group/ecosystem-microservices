
namespace Ecosystem.WalletService.Domain.Models;

public partial class WalletsModel1b
{
    public long Id { get; set; }

    public int AffiliateId { get; set; }

    public int? UserId { get; set; }

    public decimal? Credit { get; set; }

    public decimal? Debit { get; set; }

    public decimal? Deferred { get; set; }

    public bool? Status { get; set; }

    public string Concept { get; set; } = null!;

    public int? Support { get; set; }

    public DateTime Date { get; set; }

    public bool? Compression { get; set; }

    public string? Detail { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? AffiliateUserName { get; set; }

    public string? AdminUserName { get; set; }

    public string? ConceptType { get; set; }
}
