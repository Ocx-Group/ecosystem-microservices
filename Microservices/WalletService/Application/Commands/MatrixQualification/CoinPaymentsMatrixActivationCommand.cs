using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.MatrixQualification;

public record CoinPaymentsMatrixActivationCommand(IpnRequest Request, Dictionary<string, string> Headers) : IRequest<bool>;
