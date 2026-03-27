using Ecosystem.WalletService.Domain.Enums;

namespace Ecosystem.WalletService.Domain.Requests.WalletTransactionRequest;

public class WalletTransactionRequest
{
    public int AffiliateId { get; set; }
    public int? UserId { get; set; }
    public decimal? Credit { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Deferred { get; set; }
    public bool? Status { get; set; }
    public string Concept { get; set; }
    public string Support { get; set; }
    public DateTime Date { get; set; }
    public bool? Compression { get; set; }
    public string? Detail { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public DateTime? DeleteAt { get; set; }
    public string? AffiliateUserName { get; set; }
    public string? AdminUserName { get; set; }
    public WalletConceptType? ConceptType { get; set; }
    public long BrandId { get; set; }
}