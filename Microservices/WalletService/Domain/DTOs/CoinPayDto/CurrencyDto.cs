namespace Ecosystem.WalletService.Domain.DTOs.CoinPayDto;

public class CurrencyDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Code { get; set; }
    public bool IsErc { get; set; }
}