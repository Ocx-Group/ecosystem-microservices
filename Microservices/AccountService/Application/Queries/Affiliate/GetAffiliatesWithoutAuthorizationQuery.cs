using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Affiliate;

public record GetAffiliatesWithoutAuthorizationQuery : IRequest<ICollection<UsersAffiliatesDto>>;
