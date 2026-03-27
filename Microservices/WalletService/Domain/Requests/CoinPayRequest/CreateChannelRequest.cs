namespace Ecosystem.WalletService.Domain.Requests.CoinPayRequest;

public class CreateChannelRequest
{
    public int IdCurrency { get; set; }
    public int IdNetwork { get; set; }
    public int IdExternalIdentification { get; set; }
    public string? TagName { get; set; }
}