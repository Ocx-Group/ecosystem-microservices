using System.Text.Json.Serialization;

namespace Ecosystem.WalletService.Domain.Requests.PagaditoRequest;

public class PagaditoTransactionDetailRequest
{
    [JsonPropertyName("quantity")] 
    public int Quantity { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; } 
    
    [JsonPropertyName("price")] 
    public decimal? Price { get; set; }
    
    [JsonPropertyName("url_product")] 
    public string? UrlProduct { get; set; }
}