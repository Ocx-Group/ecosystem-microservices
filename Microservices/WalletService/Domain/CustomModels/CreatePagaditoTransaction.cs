using System.Text.Json.Serialization;


namespace Ecosystem.WalletService.Domain.CustomModels;

public class CreatePagaditoTransaction
{
    [JsonPropertyName("token")] 
    public string Token { get; set; } = string.Empty;
    
    [JsonPropertyName("ern")] 
    public string Ern { get; set; } = string.Empty;

    [JsonPropertyName("amount")] 
    public decimal Amount { get; set; } 

    [JsonPropertyName("details")]
    public List<PagaditoTransactionDetail> Details { get; set; } = new List<PagaditoTransactionDetail>();

    [JsonPropertyName("custom_params")]
    public Dictionary<string, string> CustomParams { get; set; } = new Dictionary<string, string>();
}