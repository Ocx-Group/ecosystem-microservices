using Ecosystem.AccountService.Application.DTOs.Role;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Role;

public record UpdateRoleCommand(int Id, string Name, string Description) : IRequest<RoleDto?>;
