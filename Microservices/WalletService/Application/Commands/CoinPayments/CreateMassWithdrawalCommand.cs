using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.CoinPayments;

public record CreateMassWithdrawalCommand(IEnumerable<CoinPaymentMassWithdrawalRequest> Requests) : IRequest<CoinPaymentWithdrawalResponse?>;
