namespace Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;

public class CoinPaymentMassWithdrawalRequest
{
    public decimal Amount { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
}