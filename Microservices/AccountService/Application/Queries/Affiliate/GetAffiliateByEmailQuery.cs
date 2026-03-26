using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Affiliate;

public record GetAffiliateByEmailQuery(string Email) : IRequest<UsersAffiliatesDto?>;
