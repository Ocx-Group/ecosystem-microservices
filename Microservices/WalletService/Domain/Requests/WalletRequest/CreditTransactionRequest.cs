namespace Ecosystem.WalletService.Domain.Requests.WalletRequest;

public class CreditTransactionRequest
{
    public int AffiliateId { get; set; }
    public int UserId { get; set; }
    public string Concept { get; set; } = string.Empty;
    public double Credit { get; set; }
    public string? AffiliateUserName { get; set; } 
    public string AdminUserName { get; set; } = string.Empty;
    public string ConceptType { get; set; } = string.Empty;
    public long BrandId { get; set; }
}