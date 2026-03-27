namespace Ecosystem.WalletService.Domain.Requests.WalletHistoryRequest;

public class WalletHistoryRequest
{
    public int Id { get; set; }
    public int AffiliateId { get; set; }
    public int? UserId { get; set; }
    public decimal Credit { get; set; }
    public decimal Debit { get; set; }
    public decimal Deferred { get; set; }
    public bool Status { get; set; }
    public string Concept { get; set; } = null!;
    public int? Support { get; set; }
    public DateTime Date { get; set; }
    public bool Compression { get; set; }
    
}