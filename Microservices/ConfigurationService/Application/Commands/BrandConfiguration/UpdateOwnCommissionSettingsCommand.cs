using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.BrandConfiguration;

public enum UpdateCommissionSettingsStatus
{
    Updated,
    NotFound,
    InvalidLevels
}

/// <summary>
/// Mirrors <see cref="UpdateOwnBrandingResult"/>: a rejected payload is reported as a
/// status so the controller can answer 400 with the operator's mistake, instead of an
/// exception that <c>ExceptionMiddleware</c> would flatten into a 500.
/// </summary>
public sealed record UpdateCommissionSettingsResult(
    UpdateCommissionSettingsStatus Status,
    CommissionSettingsDto? Settings,
    string? ValidationMessage = null);

public sealed record UpdateOwnCommissionSettingsCommand(
    UpdateCommissionSettingsRequest Settings,
    string ActorUserId,
    string ActorUserName) : IRequest<UpdateCommissionSettingsResult>;
