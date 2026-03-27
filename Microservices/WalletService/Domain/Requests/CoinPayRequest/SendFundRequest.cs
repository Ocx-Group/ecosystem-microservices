namespace Ecosystem.WalletService.Domain.Requests.CoinPayRequest;

public class SendFundRequest
{ 
    public int IdCurrency { get; set; }
    public int IdNetwork { get; set; }
    public string? Address { get; set; }
    public int Amount { get; set; }
    public string? Details { get; set; }
    public bool AmountPlusFee { get; set; }
}