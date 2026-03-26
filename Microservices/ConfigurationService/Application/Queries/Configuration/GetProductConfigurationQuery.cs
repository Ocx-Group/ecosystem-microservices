using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Queries.Configuration;

public record GetProductConfigurationQuery : IRequest<ProductConfigurationDto>;
