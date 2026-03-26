using System.Text.Json.Serialization;

namespace Ecosystem.ConfigurationService.Application.DTOs;

public class WithdrawalsWalletConfigurationDto
{
    [JsonPropertyName("minimum_amount")] public int MinimumAmount { get; set; }
    [JsonPropertyName("maximum_amount")] public int MaximumAmount { get; set; }
    [JsonPropertyName("commission_amount")] public int CommissionAmount { get; set; }
    [JsonPropertyName("activate_invoice_cancellation")] public bool ActivateInvoiceCancellation { get; set; }
}
