namespace Ecosystem.WalletService.Domain.Requests.CoinPayRequest;

public class CreateLinkRequest
{
    public string? CurrencyCode { get; set; }
    public int IdCurrencyPay { get; set; }
    public int Amount { get; set; }
    public int IdReference { get; set; }
    public string? Address { get; set; }
}