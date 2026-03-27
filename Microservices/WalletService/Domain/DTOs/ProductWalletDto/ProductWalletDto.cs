using System.Text.Json.Serialization;

namespace Ecosystem.WalletService.Domain.DTOs.ProductWalletDto;

public class ProductWalletDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("categoryId")] public int CategoryId { get; set; }
    [JsonPropertyName("salePrice")] public decimal SalePrice { get; set; }
    [JsonPropertyName("commissionableValue")] public decimal CommissionableValue { get; set; }
    [JsonPropertyName("binaryPoints")] public decimal BinaryPoints { get; set; }
    [JsonPropertyName("valuePoints")] public int ValuePoints { get; set; }
    [JsonPropertyName("tax")] public decimal Tax { get; set; }
    [JsonPropertyName("modelTwoPercentage")] public decimal? ModelTwoPercentage { get; set; }
    [JsonPropertyName("paymentGroup")] public int PaymentGroup { get; set; }
    [JsonPropertyName("acumCompMin")] public bool AcumCompMin { get; set; }
    [JsonPropertyName("productType")] public bool ProductType { get; set; }
    [JsonPropertyName("productPacks")] public bool ProductPacks { get; set; }
    [JsonPropertyName("baseAmount")] public decimal BaseAmount { get; set; }
    [JsonPropertyName("dailyPercentage")] public decimal DailyPercentage { get; set; }
    [JsonPropertyName("daysWait")] public int DaysWait { get; set; }

    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("productDiscount")] public decimal ProductDiscount { get; set; }

}