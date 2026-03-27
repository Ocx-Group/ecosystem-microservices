namespace Ecosystem.WalletService.Domain.Requests.PaymentTransaction;

public class PaymentTransactionRequest
{
    
    public string? IdTransaction { get; set; }
    public int AffiliateId { get; set; }
    public decimal Amount { get; set; }
    public string? Products { get; set; }
    public DateTime CreatedAt { get; set; }
}