using Ecosystem.WalletService.Domain.Responses;

namespace Ecosystem.WalletService.Domain.DTOs.CoinPayDto;

public class SendFundsDto
{
    public List<SendFundsResponse> SuccessfulResponses { get; set; } = new List<SendFundsResponse>();
    public List<SendFundsResponse> FailedResponses { get; set; } = new List<SendFundsResponse>();
}