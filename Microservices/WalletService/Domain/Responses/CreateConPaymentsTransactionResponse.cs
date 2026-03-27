
using Newtonsoft.Json;

namespace Ecosystem.WalletService.Domain.Responses;

public class CreateConPaymentsTransactionResponse
{

    [JsonProperty("error")]
    public string? Error { get; set; }
    [JsonProperty("result")]
    public PaymentResult? Result { get; set; }
    
}

public class PaymentResult
{
    [JsonProperty("amount")]
    public string? Amount { get; set; }
    [JsonProperty("txn_id")]
    public string? Txn_Id { get; set; }
    [JsonProperty("address")]
    public string? Address { get; set; }
    [JsonProperty("confirms_needed")]
    public string? Confirms_Needed { get; set; }
    [JsonProperty("timeout")]
    public int Timeout { get; set; }
    [JsonProperty("checkout_url")]
    public string? Checkout_Url { get; set; }
    [JsonProperty("status_url")]
    public string? Status_Url { get; set; }
    [JsonProperty("qrcode_url")]
    public string? Qrcode_Url { get; set; }
}