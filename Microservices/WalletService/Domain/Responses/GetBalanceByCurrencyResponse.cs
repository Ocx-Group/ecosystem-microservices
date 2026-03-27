using Ecosystem.WalletService.Domain.DTOs.CoinPayDto;

namespace Ecosystem.WalletService.Domain.Responses;

public class GetBalanceByCurrencyResponse
{
    public int StatusCode { get; set; }
    public int IdTypeStatusCode { get; set; }
    public string? Message { get; set; }
    public List<object>? Messages { get; set; }
    public UserBalanceDto? Data { get; set; }
}