using Ecosystem.Domain.Core.BrandConfiguration;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Queries.BrandConfiguration;

public record GetAllBrandConfigurationsQuery : IRequest<IReadOnlyList<BrandConfigurationDto>>;
