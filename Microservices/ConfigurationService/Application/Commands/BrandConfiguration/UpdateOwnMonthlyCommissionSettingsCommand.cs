using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.BrandConfiguration;

public enum UpdateMonthlyCommissionSettingsStatus
{
    Updated,
    NotFound,
    InvalidSettings
}

/// <summary>
/// Mirrors <see cref="UpdateCommissionSettingsResult"/>: a rejected payload is reported
/// as a status so the controller can answer 400 with the operator's mistake, instead of
/// an exception that <c>ExceptionMiddleware</c> would flatten into a 500.
/// </summary>
public sealed record UpdateMonthlyCommissionSettingsResult(
    UpdateMonthlyCommissionSettingsStatus Status,
    MonthlyCommissionSettingsDto? Settings,
    string? ValidationMessage = null);

public sealed record UpdateOwnMonthlyCommissionSettingsCommand(
    UpdateMonthlyCommissionSettingsRequest Settings,
    string ActorUserId,
    string ActorUserName) : IRequest<UpdateMonthlyCommissionSettingsResult>;
