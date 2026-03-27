using System.Text.Json.Serialization;

namespace Ecosystem.WalletService.Domain.Requests.PagaditoRequest;

public class AmountCollectionRequest
{
    [JsonPropertyName("total")] 
    public string? Total { get; set; } 

    [JsonPropertyName("currency")]
    public string? Currency { get; set; } 
}