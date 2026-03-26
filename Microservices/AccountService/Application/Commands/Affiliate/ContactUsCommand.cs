using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record ContactUsCommand(
    string FullName,
    string Email,
    string? PhoneNumber,
    string Subject,
    string Message
) : IRequest<bool>;
