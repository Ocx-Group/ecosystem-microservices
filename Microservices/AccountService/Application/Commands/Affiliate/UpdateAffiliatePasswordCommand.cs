using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateAffiliatePasswordCommand(int Id, string Password, string NewPassword) : IRequest<UsersAffiliatesDto?>;
