namespace Ecosystem.WalletService.Domain.Requests.WalletRequest;

public class WalletRequest
{
    public int AffiliateId { get; set; }
    public string AffiliateUserName { get; set; } = null!;
    public int PurchaseFor { get; set; }
    public string? Bank { get; set; }

    public int PaymentMethod { get; set; }
    public string? SecretKey { get; set; }
    public string? ReceiptNumber { get; set; } 
    public long BrandId { get; set; }
    public bool? DailyBonusActivation { get; set; }
    public bool? IncludeInCommissionCalculation { get; set; }

    public ICollection<ProductsRequests> ProductsList { get; set; }

}

public class ProductsRequests
{
    public int IdProduct { get; set; }
    public int Count { get; set; }
}