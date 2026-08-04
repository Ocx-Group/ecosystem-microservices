using Ecosystem.WalletService.Domain.DTOs.CoinPayDto;
using Ecosystem.WalletService.Domain.Requests.CoinPayRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.CoinPay;

/// <summary>
/// Processes a batch of withdrawal requests: resolves each affiliate's payout address,
/// sends the funds through CoinPay and records the matching wallet debit.
/// </summary>
public record SendCoinPayFundsCommand(WithDrawalRequest[] Requests) : IRequest<SendFundsDto>;
