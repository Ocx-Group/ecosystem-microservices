using Ecosystem.AccountService.Application.DTOs.Role;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Role;

public record GetRolesQuery : IRequest<List<RoleDto>>;
