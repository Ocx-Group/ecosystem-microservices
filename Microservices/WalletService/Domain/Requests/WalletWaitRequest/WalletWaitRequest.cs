namespace Ecosystem.WalletService.Domain.Requests.WalletWaitRequest;

public class WalletWaitRequest
{
    public int Id { get; set; }
    public int AffiliateId { get; set; }
    public decimal? Credit { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Bank { get; set; }
    public string? Support { get; set; }
    public DateTime? DepositDate { get; set; }
    public bool? Status { get; set; }
    public bool? Attended { get; set; }
    public DateTime Date { get; set; }
    public string? Order { get; set; }
    
}