using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Role;

public record DeleteRoleCommand(int Id) : IRequest<bool>;
