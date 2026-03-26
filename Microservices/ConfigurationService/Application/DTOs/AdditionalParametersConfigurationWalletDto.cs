using System.Text.Json.Serialization;

namespace Ecosystem.ConfigurationService.Application.DTOs;

public class AdditionalParametersConfigurationWalletDto
{
    [JsonPropertyName("minutes_validity_code")] public int MinutesValidityCode { get; set; }
    [JsonPropertyName("concept_wallet_withdrawal")] public string ConceptWalletWithdrawal { get; set; } = string.Empty;
    [JsonPropertyName("activate_confirmation_mails")] public bool ActivateConfirmationMails { get; set; }
}
