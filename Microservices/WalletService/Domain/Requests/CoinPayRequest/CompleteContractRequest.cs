namespace Ecosystem.WalletService.Domain.Requests.CoinPayRequest;

public class CompleteContractRequest
{
    public int IdWallet { get; set; }
    public int IdContract { get; set; }
}