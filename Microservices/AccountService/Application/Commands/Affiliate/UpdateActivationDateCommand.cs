using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateActivationDateCommand(int Id) : IRequest<UsersAffiliatesDto?>;
