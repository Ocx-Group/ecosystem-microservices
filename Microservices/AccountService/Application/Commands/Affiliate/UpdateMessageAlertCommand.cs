using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateMessageAlertCommand(int Id) : IRequest<UsersAffiliatesDto?>;
