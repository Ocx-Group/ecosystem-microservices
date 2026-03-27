namespace Ecosystem.WalletService.Domain.Requests.PaymentTransaction;

public class ConfirmPaymentTransactionRequest
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
}