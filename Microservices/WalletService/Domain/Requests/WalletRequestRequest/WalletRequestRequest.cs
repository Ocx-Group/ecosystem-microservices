namespace Ecosystem.WalletService.Domain.Requests.WalletRequestRequest;

public class WalletRequestRequest
{
    public int AffiliateId { get; set; }
    public string AffiliateName { get; set; } = string.Empty;
    public string VerificationCode { get; set; } = string.Empty;
    public string UserPassword { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Concept { get; set; }
    
}