using System.Text.Json.Serialization;

namespace Ecosystem.WalletService.Domain.Responses;

public class PayWithMyBalanceResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("data")]
    public List<object>? Data { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public int Code { get; set; }
}

