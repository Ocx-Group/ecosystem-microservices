using System.Text.Json.Serialization;

namespace Ecosystem.WalletService.Domain.Responses;

public class CoinPaymentWithdrawalResponse
{
    [JsonPropertyName("error")] public string? Error { get; set; }
    [JsonPropertyName("result")] public Dictionary<string, WithdrawalInfo>? Result { get; set; }
}

public class WithdrawalInfo
{
    [JsonPropertyName("error")] public string? Error { get; set; }
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("status")] public int Status { get; set; }
    [JsonPropertyName("amount")] public string? Amount { get; set; }
}