using Newtonsoft.Json;

namespace Ecosystem.WalletService.Domain.Requests.WalletRequest;

public class InvoiceDetailsTransactionRequest
{
    [JsonProperty("product_id")]
    public int ProductId { get; init; }
    [JsonProperty("payment_group_id")]
    public int PaymentGroupId { get; init; }
    
    [JsonProperty("accummin_purchase")]
    public bool AccumMinPurchase { get; init; }
    
    [JsonProperty("product_name")]
    public string ProductName { get; init; } = string.Empty;
    
    [JsonProperty("product_price")]
    public decimal ProductPrice { get; init; }
    
    [JsonProperty("product_price_btc")]
    public decimal ProductPriceBtc { get; init; }
    
    [JsonProperty("product_iva")]
    public decimal ProductIva { get; init; }
    
    [JsonProperty("product_quantity")]
    public int ProductQuantity { get; init; }
    
    [JsonProperty("product_commissionable")]
    public decimal ProductCommissionable { get; init; }
    
    [JsonProperty("binary_points")]
    public decimal BinaryPoints { get; init; }
    
    [JsonProperty("product_points")]
    public int ProductPoints { get; init; }
    
    [JsonProperty("product_discount")]
    public decimal ProductDiscount { get; init; }
    
    [JsonProperty("combination_id")]
    public int CombinationId { get; init; }
    
    [JsonProperty("product_pack")]
    public bool ProductPack { get; init; }
    [JsonProperty("base_amount")]
    public decimal BaseAmount { get; init; }
    [JsonProperty("daily_percentage")]
    public decimal DailyPercentage { get; init; }
    [JsonProperty("waiting_days")]
    public int WaitingDays { get; init; }
    [JsonProperty("days_to_pay_quantity")]
    public int DaysToPayQuantity { get; init; }
    [JsonProperty("product_start")]
    public bool ProductStart { get; init; }
    [JsonProperty("brand_id")]
    public long BrandId{get;set;}
}