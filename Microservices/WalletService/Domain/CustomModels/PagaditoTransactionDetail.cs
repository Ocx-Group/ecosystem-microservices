using System.Text.Json.Serialization;

namespace Ecosystem.WalletService.Domain.CustomModels;

public class PagaditoTransactionDetail
{
    [JsonPropertyName("quantity")] 
    public int Quantity { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; } = String.Empty;
    
    [JsonPropertyName("price")] 
    public decimal Price { get; set; }
    
    [JsonPropertyName("url_product")] 
    public string UrlProduct { get; set; } = String.Empty;
}