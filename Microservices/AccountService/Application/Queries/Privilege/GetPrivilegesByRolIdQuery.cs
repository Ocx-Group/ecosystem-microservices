using Ecosystem.AccountService.Application.DTOs.Privilege;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Privilege;

public record GetPrivilegesByRolIdQuery(int RolId) : IRequest<ICollection<PrivilegeMenuConfigurationDto>>;
