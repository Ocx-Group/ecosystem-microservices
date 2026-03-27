namespace Ecosystem.WalletService.Domain.Requests.CoinPayRequest;

public class CreateAddresRequest
{
    public int IdCurrency { get; set; }
    public int IdNetwork { get; set; }
    public int IdWallet { get; set; }
}