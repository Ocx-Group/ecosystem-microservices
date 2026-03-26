using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record RevertActivationCommand(int Id) : IRequest<UsersAffiliatesDto?>;
