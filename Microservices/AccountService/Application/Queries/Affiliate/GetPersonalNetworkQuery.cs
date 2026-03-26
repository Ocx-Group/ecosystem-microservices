using Ecosystem.AccountService.Domain.Models.CustomModels;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Affiliate;

public record GetPersonalNetworkQuery(int UserId) : IRequest<List<AffiliatePersonalNetwork>>;
