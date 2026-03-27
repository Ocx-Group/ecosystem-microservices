namespace Ecosystem.WalletService.Domain.CustomModels;

public class AffiliateBalance
{
    public long AffiliateId { get; set; }
    public string? AffiliateUserName { get; set; }
    public decimal? Balance { get; set; }
}