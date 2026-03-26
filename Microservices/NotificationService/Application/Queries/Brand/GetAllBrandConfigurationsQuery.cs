using Ecosystem.NotificationService.Application.DTOs;
using MediatR;

namespace Ecosystem.NotificationService.Application.Queries.Brand;

public record GetAllBrandConfigurationsQuery : IRequest<ICollection<BrandConfigurationDto>>;
