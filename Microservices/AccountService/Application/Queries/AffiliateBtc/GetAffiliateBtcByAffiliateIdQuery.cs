using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.AffiliateBtc;

public record GetAffiliateBtcByAffiliateIdQuery(int AffiliateId) : IRequest<List<AffiliateBtcDto>?>;
