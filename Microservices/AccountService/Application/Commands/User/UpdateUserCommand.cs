using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.User;

public record UpdateUserCommand(
    int Id,
    int RolId,
    string? UserName,
    string? Email,
    string? Name,
    string? LastName,
    string? Phone,
    string? Address,
    string? Observation,
    bool Status
) : IRequest<UserDto?>;
