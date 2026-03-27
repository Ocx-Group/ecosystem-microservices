using Ecosystem.Domain.Core.BrandConfiguration;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Queries.BrandConfiguration;

public record GetBrandConfigurationByBrandIdQuery(long BrandId) : IRequest<BrandConfigurationDto?>;
