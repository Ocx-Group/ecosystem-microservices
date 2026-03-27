using System.Text.Json.Serialization;

namespace Ecosystem.WalletService.Domain.Responses;

public class GetCoinBalancesResponse
{
    public string? Error { get; set; }
    public Dictionary<string, CoinInfo> Result { get; set; }

}

public class CoinInfo
{
    [JsonPropertyName("balance")]
    public long Balance { get; set; }

    [JsonPropertyName("balancef")]
    public string BalanceF { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("coin_status")]
    public string CoinStatus { get; set; } = string.Empty;
}