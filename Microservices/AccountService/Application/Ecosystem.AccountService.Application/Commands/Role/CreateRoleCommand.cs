using Ecosystem.AccountService.Application.DTOs.Role;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Role;

public record CreateRoleCommand(string Name, string Description) : IRequest<RoleDto?>;
