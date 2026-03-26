using System.Text.Json.Serialization;

namespace Ecosystem.ConfigurationService.Application.DTOs;

public class CompensationPlansConfigurationDto
{
    [JsonPropertyName("automatic_activation")] public bool AutomaticActivation { get; set; }
    [JsonPropertyName("automatic_qualification")] public bool AutomaticQualification { get; set; }
    [JsonPropertyName("automatic_incentive_calculation")] public bool AutomaticIncentiveCalculation { get; set; }
    [JsonPropertyName("automatic_commission_calculation")] public bool AutomaticCommissionCalculation { get; set; }
}
