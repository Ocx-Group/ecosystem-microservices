using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Queries.AffiliateBtc;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.AffiliateBtc;

public class GetAffiliateBtcByAffiliateIdHandler
    : IRequestHandler<GetAffiliateBtcByAffiliateIdQuery, List<AffiliateBtcDto>?>
{
    private readonly IAffiliateBtcRepository _affiliateBtcRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public GetAffiliateBtcByAffiliateIdHandler(
        IAffiliateBtcRepository affiliateBtcRepository,
        ITenantContext tenantContext,
        IMapper mapper)
    {
        _affiliateBtcRepository = affiliateBtcRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<List<AffiliateBtcDto>?> Handle(
        GetAffiliateBtcByAffiliateIdQuery request, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.TenantId;

        var trcAddress = await _affiliateBtcRepository
            .GetTrc20AddressByAffiliateIdAndNetworkId(request.AffiliateId, brandId);
        var bnbAddress = await _affiliateBtcRepository
            .GetBnbAddressByAffiliateIdAndNetworkId(request.AffiliateId, brandId);

        var result = new List<AffiliateBtcDto>();

        var bnbDto = _mapper.Map<AffiliateBtcDto>(bnbAddress);
        var trcDto = _mapper.Map<AffiliateBtcDto>(trcAddress);

        result.Add(bnbDto);
        result.Add(trcDto);

        return result;
    }
}
