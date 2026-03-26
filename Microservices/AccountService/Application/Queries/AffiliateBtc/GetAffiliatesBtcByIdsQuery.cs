using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.AffiliateBtc;

public record GetAffiliatesBtcByIdsQuery(long[] Ids) : IRequest<IEnumerable<AffiliateBtcDto>?>;
