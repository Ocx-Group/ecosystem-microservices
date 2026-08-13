using Ecosystem.WalletService.Domain.DTOs.MonthlyCommissionDto;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.Wallet;

public enum CalculateMonthlyCommissionsStatus
{
    Completed,
    BrandNotFound,
    Disabled,
    InvalidRequest
}

/// <summary>
/// A rejected run is reported as a status rather than an exception, so the controller
/// can tell the operator what is wrong with the period or the parameters instead of
/// letting <c>ExceptionMiddleware</c> flatten it into a 500.
/// </summary>
public sealed record CalculateMonthlyCommissionsResult(
    CalculateMonthlyCommissionsStatus Status,
    MonthlyCommissionResultDto? Result,
    string? ValidationMessage = null);

public sealed record CalculateMonthlyCommissionsCommand(
    CalculateMonthlyCommissionsRequest Request,
    string ActorUserId,
    string ActorUserName) : IRequest<CalculateMonthlyCommissionsResult>;
