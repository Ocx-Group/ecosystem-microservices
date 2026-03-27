using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.WalletService.Domain.Responses.BaseResponses;

namespace Ecosystem.WalletService.Domain.DTOs.CoinPayDto;

public class WithdrawalDto
{
    public ServicesResponse? Response { get; set; }
    public WithdrawalStatus Status { get; set; }

}