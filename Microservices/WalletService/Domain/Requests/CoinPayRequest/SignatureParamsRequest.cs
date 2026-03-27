namespace Ecosystem.WalletService.Domain.Requests.CoinPayRequest;

public class SignatureParamsRequest
{
    public int IdUser { get; set; }
    public int IdTransaction { get; set; }
    public string DynamicKey { get; set; } = string.Empty;
    public string? IncomingSignature { get; set; }
}