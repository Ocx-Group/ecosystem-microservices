using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.ModelProcess;

public record ExecuteProcessSecondCommand : IRequest<GradingResponse?>;
