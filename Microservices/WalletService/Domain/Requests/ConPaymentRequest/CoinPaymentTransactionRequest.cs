namespace Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;

public class CoinPaymentTransactionRequest
{
    public int Id { get; set; }
    public string? IdTransaction { get; set; }
    public int AffiliateId { get; set; }
    public decimal Amount { get; set; }
    public decimal AmountReceived { get; set; }
    public string? Products { get; set; }
    public bool Acredited { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}