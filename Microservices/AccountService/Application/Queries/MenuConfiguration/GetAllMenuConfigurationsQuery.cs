using Ecosystem.AccountService.Application.DTOs.MenuConfiguration;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.MenuConfiguration;

public record GetAllMenuConfigurationsQuery() : IRequest<ICollection<MenuConfigurationDto>>;
