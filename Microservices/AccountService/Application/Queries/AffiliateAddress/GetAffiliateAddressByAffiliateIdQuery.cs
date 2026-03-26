using Ecosystem.AccountService.Application.DTOs.AffiliateAddress;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.AffiliateAddress;

public record GetAffiliateAddressByAffiliateIdQuery(int AffiliateId) : IRequest<IEnumerable<AffiliateAddressDto>?>;
