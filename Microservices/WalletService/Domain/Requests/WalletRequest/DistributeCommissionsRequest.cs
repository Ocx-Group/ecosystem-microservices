namespace Ecosystem.WalletService.Domain.Requests.WalletRequest;

public class DistributeCommissionsRequest
{
    public int AffiliateId { get; set; }
    public decimal InvoiceAmount { get; set; }
    public long BrandId { get; set; }
    
    public string AdminUserName { get; set; } = string.Empty;
    public decimal[] LevelPercentages { get; set; }
}