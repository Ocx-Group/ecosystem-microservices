using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record ValidationCodeCommand(int UserId, string Code, string Password) : IRequest<bool>;
