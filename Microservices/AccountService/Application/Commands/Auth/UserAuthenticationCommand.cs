using Ecosystem.AccountService.Application.DTOs.Auth;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Auth;

public record UserAuthenticationCommand(
    string UserName,
    string Password,
    string? BrowserInfo,
    string? OperatingSystem,
    string? IpAddress
) : IRequest<AuthResultDto>;
