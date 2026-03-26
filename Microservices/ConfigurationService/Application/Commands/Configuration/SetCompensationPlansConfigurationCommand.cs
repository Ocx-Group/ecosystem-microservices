using System.Text.Json.Serialization;
using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.Configuration;

public record SetCompensationPlansConfigurationCommand : IRequest<CompensationPlansConfigurationDto>
{
    [JsonPropertyName("automatic_activation")] public bool AutomaticActivation { get; init; }
    [JsonPropertyName("automatic_qualification")] public bool AutomaticQualification { get; init; }
    [JsonPropertyName("automatic_incentive_calculation")] public bool AutomaticIncentiveCalculation { get; init; }
    [JsonPropertyName("automatic_commission_calculation")] public bool AutomaticCommissionCalculation { get; init; }
}
