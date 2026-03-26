using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.User;

public record CreateUserCommand(
    int RolId,
    string UserName,
    string Email,
    string Password,
    string? Name,
    string? LastName,
    string? Phone,
    string? Address,
    string? Observation,
    bool Status
) : IRequest<UserDto?>;
