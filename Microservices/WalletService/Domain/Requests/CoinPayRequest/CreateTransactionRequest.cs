using System.Text.Json.Serialization;

using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;

namespace Ecosystem.WalletService.Domain.Requests.CoinPayRequest;

public class CreateTransactionRequest
{
    [JsonPropertyName("affiliateId")] public int AffiliateId { get; set; }
    
    [JsonPropertyName("userName")] public string UserName { get; set; } = string.Empty;
    [JsonPropertyName("amount")] public int Amount { get; set; }
    [JsonPropertyName("products")] public List<ProductRequest>? Products { get; set; }
    [JsonPropertyName("networkId")] public int NetworkId { get; set; }
    [JsonPropertyName("currencyId")] public int CurrencyId { get; set; }
}