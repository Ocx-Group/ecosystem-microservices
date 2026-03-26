using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.User;

public record UpdatePasswordCommand(
    int Id,
    string Password,
    string NewPassword
) : IRequest<UserDto?>;
