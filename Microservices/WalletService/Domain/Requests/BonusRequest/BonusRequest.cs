namespace Ecosystem.WalletService.Domain.Requests.BonusRequest;

public class BonusRequest
{
    public int InvoiceId { get; set; }
    public int AffiliateId { get; set; }
    public decimal Amount { get; set; }
    public string Comment { get; set; } = string.Empty;
}