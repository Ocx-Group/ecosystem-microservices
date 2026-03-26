using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Queries.AffiliateBtc;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.AffiliateBtc;

public class GetAffiliatesBtcByIdsHandler
    : IRequestHandler<GetAffiliatesBtcByIdsQuery, IEnumerable<AffiliateBtcDto>?>
{
    private readonly IAffiliateBtcRepository _affiliateBtcRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public GetAffiliatesBtcByIdsHandler(
        IAffiliateBtcRepository affiliateBtcRepository,
        ITenantContext tenantContext,
        IMapper mapper)
    {
        _affiliateBtcRepository = affiliateBtcRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AffiliateBtcDto>?> Handle(
        GetAffiliatesBtcByIdsQuery request, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.TenantId;
        var affiliatesBtc = await _affiliateBtcRepository.GetAllAffiliatesBtcByIdsAsync(request.Ids, brandId);

        return _mapper.Map<IEnumerable<AffiliateBtcDto>>(affiliatesBtc);
    }
}
