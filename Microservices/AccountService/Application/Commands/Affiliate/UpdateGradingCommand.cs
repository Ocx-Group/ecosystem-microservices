using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateGradingCommand(int UserId, int GradingId) : IRequest<bool>;
