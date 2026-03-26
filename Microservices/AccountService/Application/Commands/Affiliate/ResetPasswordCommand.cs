using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record ResetPasswordCommand(string VerificationCode, string Password) : IRequest<bool>;
