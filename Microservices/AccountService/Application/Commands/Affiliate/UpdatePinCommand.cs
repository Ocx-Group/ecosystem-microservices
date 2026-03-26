using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdatePinCommand(int Id, string Password, string? SecurityPin) : IRequest<UsersAffiliatesDto?>;
