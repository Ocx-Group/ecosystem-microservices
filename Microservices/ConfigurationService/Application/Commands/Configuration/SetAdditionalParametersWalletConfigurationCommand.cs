using System.Text.Json.Serialization;
using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.Configuration;

public record SetAdditionalParametersWalletConfigurationCommand : IRequest<AdditionalParametersConfigurationWalletDto>
{
    [JsonPropertyName("minutes_validity_code")] public int MinutesValidityCode { get; init; }
    [JsonPropertyName("concept_wallet_withdrawal")] public string ConceptWalletWithdrawal { get; init; } = string.Empty;
    [JsonPropertyName("activate_confirmation_mails")] public bool ActivateConfirmationMails { get; init; }
}
