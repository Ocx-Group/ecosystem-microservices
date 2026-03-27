namespace Ecosystem.WalletService.Domain.Requests.CoinPayRequest;

public class WalletWithDetailRequest
{
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? CountryCode { get; set; }
    public string? Email { get; set; }
}