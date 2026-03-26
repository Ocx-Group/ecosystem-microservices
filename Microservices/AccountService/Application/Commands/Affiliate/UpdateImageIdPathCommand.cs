using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateImageIdPathCommand(int Id, string ImageIdPath) : IRequest<UsersAffiliatesDto?>;
