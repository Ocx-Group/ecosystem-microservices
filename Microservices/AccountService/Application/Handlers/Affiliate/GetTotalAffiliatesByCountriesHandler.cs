using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Queries.Affiliate;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class GetTotalAffiliatesByCountriesHandler
    : IRequestHandler<GetTotalAffiliatesByCountriesQuery, IEnumerable<CountryNetworkDto>>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public GetTotalAffiliatesByCountriesHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext, IMapper mapper)
    {
        _repo = repo;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CountryNetworkDto>> Handle(GetTotalAffiliatesByCountriesQuery request, CancellationToken ct)
    {
        var result = await _repo.TotalAffiliatesByCountry(_tenantContext.TenantId);
        return !result.Any() ? Enumerable.Empty<CountryNetworkDto>() : _mapper.Map<IEnumerable<CountryNetworkDto>>(result);
    }
}
