using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record EmailConfirmationCommand(string UserName) : IRequest<bool>;
