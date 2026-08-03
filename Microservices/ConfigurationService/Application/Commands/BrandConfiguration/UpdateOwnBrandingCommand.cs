using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.BrandConfiguration;

public enum UpdateOwnBrandingStatus
{
    Updated,
    NotFound,
    InvalidHost,
    HostConflict
}

/// <summary>
/// Conflicts are reported as a status instead of an exception so the controller
/// can answer with a precise HTTP code. An exception would be turned into a 500
/// by <c>ExceptionMiddleware</c> and hide an operator mistake behind a fault.
/// </summary>
public sealed record UpdateOwnBrandingResult(
    UpdateOwnBrandingStatus Status,
    BrandingAdministrationDto? Branding);

public sealed record UpdateOwnBrandingCommand(
    UpdateOwnBrandingRequest Branding,
    string ActorUserId,
    string ActorUserName) : IRequest<UpdateOwnBrandingResult>;
