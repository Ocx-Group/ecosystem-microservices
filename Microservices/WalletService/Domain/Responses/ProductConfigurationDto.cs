using System.Text.Json.Serialization;

namespace Ecosystem.WalletService.Domain.Responses;


public class GetConfigurationResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    
    [JsonPropertyName("data")]
    public ProductConfigurationDto Data { get; set; }
    
    [JsonPropertyName("message")]
    public string Message { get; set; }
    
    [JsonPropertyName("code")]
    public int Code { get; set; }
}

public class ProductConfigurationDto
{
    [JsonPropertyName("activate_shipping_system")]
    public bool ActivateShippingSystem { get; set; }

    [JsonPropertyName("activate_passive_payments_module")]
    public bool ActivatePassivePaymentsModule { get; set; }

    [JsonPropertyName("activate_public_shop")]
    public bool ActivatePublicShop { get; set; }

    [JsonPropertyName("currency_symbol")] public string? CurrencySymbol { get; set; }

    [JsonPropertyName("symbol_commissionable_value")]
    public string? SymbolCommissionableValue { get; set; }

    [JsonPropertyName("symbol_points_qualify")]
    public string? SymbolPointsQualify { get; set; }

    [JsonPropertyName("binary_points_symbol")]
    public string? BinaryPointsSymbol { get; set; }

    [JsonPropertyName("new_product_label")]
    public int NewProductLabel { get; set; }
}