using System.Text.Json.Serialization;

namespace Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;

public class ConPaymentRequest
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("currency1")]
    public string Currency1 { get; set; } = string.Empty;

    [JsonPropertyName("currency2")]
    public string Currency2 { get; set; } = string.Empty;

    [JsonPropertyName("buyer_email")]
    public string BuyerEmail { get; set; } = string.Empty;

    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;

    [JsonPropertyName("buyer_name")]
    public string? BuyerName { get; set; }

    [JsonPropertyName("item_name")]
    public string? ItemName { get; set; }

    [JsonPropertyName("item_number")]
    public string? ItemNumber { get; set; }

    [JsonPropertyName("invoice")]
    public string? Invoice { get; set; }

    [JsonPropertyName("custom")]
    public string? Custom { get; set; }

    [JsonPropertyName("ipn_url")]
    public string? IpnUrl { get; set; }

    [JsonPropertyName("success_url")]
    public string? SuccessUrl { get; set; }

    [JsonPropertyName("cancel_url")]
    public string? CancelUrl { get; set; }

    [JsonPropertyName("products")]
    public List<ProductRequest> Products { get; set; }
}