using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record SendEmailToChangePasswordCommand(string Email) : IRequest<bool>;
