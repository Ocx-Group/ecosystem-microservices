using System.Text.Json.Serialization;

namespace Ecosystem.ConfigurationService.Application.DTOs;

public class MatrixConfigurationDto
{
    [JsonPropertyName("uni_level_matrix")] public bool UniLevelMatrix { get; set; }
    [JsonPropertyName("force_matrix")] public bool ForceMatrix { get; set; }
    [JsonPropertyName("binary_matrix")] public bool BinaryMatrix { get; set; }
    [JsonPropertyName("affiliates_front_num")] public int AffiliatesFrontNum { get; set; }
    [JsonPropertyName("software_millennium_front_num")] public int SoftwareMillenniumFrontNum { get; set; }
}
