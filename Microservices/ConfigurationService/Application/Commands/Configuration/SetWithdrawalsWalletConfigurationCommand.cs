using System.Text.Json.Serialization;
using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.Configuration;

public record SetWithdrawalsWalletConfigurationCommand : IRequest<WithdrawalsWalletConfigurationDto>
{
    [JsonPropertyName("minimum_amount")] public int MinimumAmount { get; init; }
    [JsonPropertyName("maximum_amount")] public int MaximumAmount { get; init; }
    [JsonPropertyName("commission_amount")] public int CommissionAmount { get; init; }
    [JsonPropertyName("activate_invoice_cancellation")] public bool ActivateInvoiceCancellation { get; init; }
}
