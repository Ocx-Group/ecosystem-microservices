using Newtonsoft.Json;

namespace Ecosystem.WalletService.Domain.Requests.CoinPayRequest;

public class WithDrawalRequest
{
    [JsonProperty("id")] 
    public int Id { get; set; }
    
    [JsonProperty("affiliateId")] 
    public int AffiliateId { get; set; }
    
    [JsonProperty("amount")] 
    public int Amount { get; set; }
}