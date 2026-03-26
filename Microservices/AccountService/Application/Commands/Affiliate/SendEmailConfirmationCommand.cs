using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record SendEmailConfirmationCommand(int Id) : IRequest<bool>;
