using System.Text.Json.Serialization;

namespace Ecosystem.WalletService.Domain.Requests.TransferBalanceRequest;

public class TransferBalanceRequest
{
    [JsonPropertyName("fromAffiliateId")]
    public int FromAffiliateId { get; set; }
    
    [JsonPropertyName("fromUserName")]
    public string FromUserName { get; set; } = string.Empty;
    
    [JsonPropertyName("toUserName")]
    public string ToUserName { get; set; } = string.Empty;
    
    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }

    [JsonPropertyName("securityCode")]
    public string SecurityCode { get; set; } = string.Empty;
}