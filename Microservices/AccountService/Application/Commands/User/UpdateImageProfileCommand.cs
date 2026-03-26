using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.User;

public record UpdateImageProfileCommand(
    int Id,
    string ImageProfileUrl
) : IRequest<UserDto?>;
