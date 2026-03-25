using Ecosystem.AccountService.Application.DTOs.Role;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Role;

public record GetRoleByIdQuery(int Id) : IRequest<RoleDto?>;
