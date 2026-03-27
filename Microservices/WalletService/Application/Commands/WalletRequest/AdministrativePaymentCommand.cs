using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletRequest;

public record AdministrativePaymentCommand(long[] RequestIds) : IRequest<ResultResponse<int>>;
