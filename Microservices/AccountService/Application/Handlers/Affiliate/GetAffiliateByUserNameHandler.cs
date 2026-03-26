using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Queries.Affiliate;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class GetAffiliateByUserNameHandler : IRequestHandler<GetAffiliateByUserNameQuery, UsersAffiliatesDto?>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public GetAffiliateByUserNameHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext, IMapper mapper)
    {
        _repo = repo;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<UsersAffiliatesDto?> Handle(GetAffiliateByUserNameQuery request, CancellationToken ct)
    {
        var user = await _repo.GetAffiliateByUserNameAsync(request.UserName, _tenantContext.TenantId);
        return user is null ? null : _mapper.Map<UsersAffiliatesDto>(user);
    }
}
