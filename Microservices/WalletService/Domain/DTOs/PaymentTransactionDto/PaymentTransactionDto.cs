namespace Ecosystem.WalletService.Domain.DTOs.PaymentTransactionDto;

public class PaymentTransactionDto
{
    public long Id { get; set; }
    public string? UserName { get; set; } = string.Empty;
    public string IdTransaction { get; set; } = string.Empty;
    public int AffiliateId { get; set; }
    public decimal Amount { get; set; }
    public decimal AmountReceived { get; set; }
    public string Products { get; set; } = string.Empty;
    public int Status { get; set; }
    public bool Acredited { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? PaymentMethod { get; set; }
}