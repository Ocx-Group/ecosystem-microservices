using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Queries.BrandConfiguration;

public sealed record GetOwnBrandingQuery : IRequest<BrandingAdministrationDto?>;
