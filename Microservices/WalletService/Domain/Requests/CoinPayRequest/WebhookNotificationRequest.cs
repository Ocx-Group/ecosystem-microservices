namespace Ecosystem.WalletService.Domain.Requests.CoinPayRequest;

public class WebhookNotificationRequest
{
    public int IdUser { get; set; }
    public int IdWallet { get; set; }
    public int IdTransaction { get; set; }
    public string IdExternalReference { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? TxtId { get; set; }
    public decimal Amount { get; set; }
    public decimal Fee { get; set; }
    public decimal ExchangeRate { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public Currency? Currency { get; set; }
    public UserChannel? UserChannel { get; set; }
    public TransactionType? TransactionType { get; set; }
    public TransactionStatus TransactionStatus { get; set; } = new TransactionStatus();
    public string? Description { get; set; }
    public int NumberAttemps { get; set; }
    public int WalletConfirmations { get; set; }
    public DateTime CreateDate { get; set; }
}