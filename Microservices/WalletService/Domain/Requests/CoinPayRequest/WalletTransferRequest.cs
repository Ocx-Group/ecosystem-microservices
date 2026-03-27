namespace Ecosystem.WalletService.Domain.Requests.CoinPayRequest;

public class WalletTransferRequest
{
    public int IdWalletFrom { get; set; }
    public int IdWalletTo { get; set; }
    public int IdCurrency { get; set; }
    public double Amount { get; set; }
    public string? Detail { get; set; }
}