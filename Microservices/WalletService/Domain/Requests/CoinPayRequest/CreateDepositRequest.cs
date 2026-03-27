namespace Ecosystem.WalletService.Domain.Requests.CoinPayRequest;

public class CreateDepositRequest
{
    public int IdUser { get; set; }
    public int IdCurrency { get; set; }
    public int Amount { get; set; }
    public string? Description { get; set; }
    public string? IdReference { get; set; }
}