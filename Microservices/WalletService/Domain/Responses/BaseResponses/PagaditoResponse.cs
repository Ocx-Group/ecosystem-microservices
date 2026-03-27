using System.Text.Json.Serialization;

namespace Ecosystem.WalletService.Domain.Responses.BaseResponses;

public class PagaditoResponse
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }
    
    [JsonPropertyName("message")]
    public string? Message { get; set; } 
    
    [JsonPropertyName("value")] 
    public string? Value { get; set; }
    
    [JsonPropertyName("datetime")]
    public string? Datetime { get; set; } 
}