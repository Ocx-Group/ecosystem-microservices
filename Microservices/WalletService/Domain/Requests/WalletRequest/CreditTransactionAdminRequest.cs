namespace Ecosystem.WalletService.Domain.Requests.WalletRequest;

public class CreditTransactionAdminRequest
{
    public int AffiliateId { get; set; }
    public double Amount { get; set; }
}