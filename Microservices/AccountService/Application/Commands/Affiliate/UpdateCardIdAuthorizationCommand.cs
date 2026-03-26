using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateCardIdAuthorizationCommand(int Id, int Option) : IRequest<UsersAffiliatesDto?>;
