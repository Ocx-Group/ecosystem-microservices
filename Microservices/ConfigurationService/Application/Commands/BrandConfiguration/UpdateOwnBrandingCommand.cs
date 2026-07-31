using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.BrandConfiguration;

public sealed record UpdateOwnBrandingCommand(
    UpdateOwnBrandingRequest Branding,
    string ActorUserId,
    string ActorUserName) : IRequest<BrandingAdministrationDto?>;
