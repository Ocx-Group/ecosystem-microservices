using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.ModelProcess;

public record ExecuteProcessFirstCommand : IRequest<GradingResponse?>;
