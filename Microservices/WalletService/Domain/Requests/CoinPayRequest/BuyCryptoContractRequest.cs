namespace Ecosystem.WalletService.Domain.Requests.CoinPayRequest;

public class BuyCryptoContractRequest
{
    public int IdWallet { get; set; }
    public int IdCurrencyFrom { get; set; }
    public int IdCurrencyTo { get; set; }
    public double Amount { get; set; }
    public bool IsAmountTo { get; set; }
}