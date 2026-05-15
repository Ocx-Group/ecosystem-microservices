using Ecosystem.AccountService.Application.DTOs.Auth;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Auth;

public record GoogleAuthenticationCommand(
    string IdToken,
    string? ReferralUserName,
    int? Country,
    string? Phone,
    bool TermsConditions,
    string? BrowserInfo,
    string? OperatingSystem,
    string? IpAddress
) : IRequest<AuthResultDto>;
