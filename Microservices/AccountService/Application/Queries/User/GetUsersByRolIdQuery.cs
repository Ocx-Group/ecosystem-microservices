using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.User;

public record GetUsersByRolIdQuery(int RolId) : IRequest<ICollection<UserDto>>;
