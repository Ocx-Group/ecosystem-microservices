namespace Ecosystem.WalletService.Domain.Requests.WalletRequest;

public class WalletPaymentRequest
{
    public long Id { get; set; }
    public int AffiliateId { get; set; }
    public string AffiliateUserName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}