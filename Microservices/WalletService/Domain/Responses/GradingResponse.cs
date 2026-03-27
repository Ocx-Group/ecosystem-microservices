using System.Text.Json.Serialization;

using Ecosystem.WalletService.Domain.DTOs.GradingDto;

namespace Ecosystem.WalletService.Domain.Responses;

public class GradingResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("data")] public List<GradingDto> Data { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;

    [JsonPropertyName("code")] public int Code { get; set; }
}