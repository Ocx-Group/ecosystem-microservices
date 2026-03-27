using System.Text.Json.Serialization;


namespace Ecosystem.WalletService.Domain.Requests.PagaditoRequest;

public class ResourceCollectionRequest
{
    [JsonPropertyName("token")] 
    public string? Token { get; set; } 

    [JsonPropertyName("ern")]
    public string? Ern { get; set; } 

    [JsonPropertyName("create_timestamp")]
    public string? CreateTimestamp { get; set; }

    [JsonPropertyName("amount")]
    public AmountCollectionRequest? Amount { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; } 

    [JsonPropertyName("update_timestamp")]
    public string? UpdateTimestamp { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; } 

    [JsonPropertyName("reference")]
    public string? Reference { get; set; } 

    [JsonPropertyName("items_list")]
    public List<PagaditoTransactionDetailRequest>? ItemsList { get; set; }
}