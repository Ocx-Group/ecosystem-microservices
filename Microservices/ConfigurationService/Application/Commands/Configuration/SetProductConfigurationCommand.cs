using System.Text.Json.Serialization;
using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.Configuration;

public record SetProductConfigurationCommand : IRequest<ProductConfigurationDto>
{
    [JsonPropertyName("activate_shipping_system")] public bool ActivateShippingSystem { get; init; }
    [JsonPropertyName("activate_passive_payments_module")] public bool ActivatePassivePaymentsModule { get; init; }
    [JsonPropertyName("activate_public_shop")] public bool ActivatePublicShop { get; init; }
    [JsonPropertyName("currency_symbol")] public string CurrencySymbol { get; init; } = string.Empty;
    [JsonPropertyName("symbol_commissionable_value")] public string SymbolCommissionableValue { get; init; } = string.Empty;
    [JsonPropertyName("symbol_points_qualify")] public string SymbolPointsQualify { get; init; } = string.Empty;
    [JsonPropertyName("binary_points_symbol")] public string BinaryPointsSymbol { get; init; } = string.Empty;
    [JsonPropertyName("new_product_label")] public int NewProductLabel { get; init; }
}
