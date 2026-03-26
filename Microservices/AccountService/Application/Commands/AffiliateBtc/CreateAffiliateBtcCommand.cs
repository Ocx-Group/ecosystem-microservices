using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.AffiliateBtc;

public record CreateAffiliateBtcCommand(
    int AffiliateId,
    string Password,
    string VerificationCode,
    string? Trc20Address,
    string? BscAddress
) : IRequest<List<AffiliateBtcDto>>;
