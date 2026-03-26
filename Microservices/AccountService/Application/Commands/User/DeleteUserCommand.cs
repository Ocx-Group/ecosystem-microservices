using MediatR;

namespace Ecosystem.AccountService.Application.Commands.User;

public record DeleteUserCommand(int Id) : IRequest<bool>;
