using System.Text.Json.Serialization;

namespace Ecosystem.ConfigurationService.Application.DTOs;

public class ProductConfigurationDto
{
    [JsonPropertyName("activate_shipping_system")] public bool ActivateShippingSystem { get; set; }
    [JsonPropertyName("activate_passive_payments_module")] public bool ActivatePassivePaymentsModule { get; set; }
    [JsonPropertyName("activate_public_shop")] public bool ActivatePublicShop { get; set; }
    [JsonPropertyName("currency_symbol")] public string? CurrencySymbol { get; set; }
    [JsonPropertyName("symbol_commissionable_value")] public string? SymbolCommissionableValue { get; set; }
    [JsonPropertyName("symbol_points_qualify")] public string? SymbolPointsQualify { get; set; }
    [JsonPropertyName("binary_points_symbol")] public string? BinaryPointsSymbol { get; set; }
    [JsonPropertyName("new_product_label")] public int NewProductLabel { get; set; }
}
