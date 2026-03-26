using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateAffiliateImageCommand(int Id, string ImageProfileUrl) : IRequest<UsersAffiliatesDto?>;
