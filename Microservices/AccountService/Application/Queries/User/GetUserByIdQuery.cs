using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.User;

public record GetUserByIdQuery(int Id) : IRequest<UserDto?>;
