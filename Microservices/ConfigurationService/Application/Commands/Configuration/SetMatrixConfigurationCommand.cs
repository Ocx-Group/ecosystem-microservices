using System.Text.Json.Serialization;
using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.Configuration;

public record SetMatrixConfigurationCommand : IRequest<MatrixConfigurationDto>
{
    [JsonPropertyName("uni_level_matrix")] public bool UniLevelMatrix { get; init; }
    [JsonPropertyName("force_matrix")] public bool ForceMatrix { get; init; }
    [JsonPropertyName("binary_matrix")] public bool BinaryMatrix { get; init; }
    [JsonPropertyName("affiliates_front_num")] public int AffiliatesFrontNum { get; init; }
    [JsonPropertyName("software_millennium_front_num")] public int SoftwareMillenniumFrontNum { get; init; }
}
