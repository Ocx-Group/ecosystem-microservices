using MediatR;

namespace Ecosystem.ConfigurationService.Application.Queries.Configuration;

public record GetPointsConfigurationQuery : IRequest<int?>;
