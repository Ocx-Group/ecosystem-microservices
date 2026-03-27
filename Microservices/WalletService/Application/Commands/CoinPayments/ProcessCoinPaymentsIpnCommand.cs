using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.CoinPayments;

public record ProcessCoinPaymentsIpnCommand(IpnRequest Request, Dictionary<string, string> Headers) : IRequest<bool>;
