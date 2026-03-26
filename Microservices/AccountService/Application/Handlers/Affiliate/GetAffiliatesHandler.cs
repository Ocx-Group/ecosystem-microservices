using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Queries.Affiliate;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class GetAffiliatesHandler : IRequestHandler<GetAffiliatesQuery, ICollection<UsersAffiliatesDto>>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public GetAffiliatesHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext, IMapper mapper)
    {
        _repo = repo;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<ICollection<UsersAffiliatesDto>> Handle(GetAffiliatesQuery request, CancellationToken ct)
    {
        var affiliates = await _repo.GetAffiliatesAsync(_tenantContext.TenantId);
        return _mapper.Map<ICollection<UsersAffiliatesDto>>(affiliates);
    }
}
