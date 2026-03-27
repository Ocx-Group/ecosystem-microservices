using System.Text.Json.Serialization;

using Ecosystem.WalletService.Domain.DTOs.WalletDto;

namespace Ecosystem.WalletService.Domain.Responses;

public class GradingsResponse
{
    public class ProductsResponse
    {
        [JsonPropertyName("success")] public bool Success { get; set; }
        [JsonPropertyName("data")] public ICollection<WalletDto> Data { get; set; } = new List<WalletDto>();

        [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;

        [JsonPropertyName("code")] public int Code { get; set; }
    }
}