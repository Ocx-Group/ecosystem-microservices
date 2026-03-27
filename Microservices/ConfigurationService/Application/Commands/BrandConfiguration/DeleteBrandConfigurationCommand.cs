using Ecosystem.Domain.Core.BrandConfiguration;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.BrandConfiguration;

public record DeleteBrandConfigurationCommand(long BrandId) : IRequest<BrandConfigurationDto?>;
