namespace Ecosystem.WalletService.Domain.Requests.WalletRequestRequest;

public class WalletRequestRevertTransaction
{
    public int AffiliateId { get; set; }
    public int InvoiceId { get; set; }
    public string Concept { get; set; } = string.Empty;
}