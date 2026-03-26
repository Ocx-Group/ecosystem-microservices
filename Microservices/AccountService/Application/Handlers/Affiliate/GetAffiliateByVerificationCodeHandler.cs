using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Queries.Affiliate;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class GetAffiliateByVerificationCodeHandler
    : IRequestHandler<GetAffiliateByVerificationCodeQuery, UsersAffiliatesDto?>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public GetAffiliateByVerificationCodeHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext, IMapper mapper)
    {
        _repo = repo;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<UsersAffiliatesDto?> Handle(GetAffiliateByVerificationCodeQuery request, CancellationToken ct)
    {
        var result = await _repo.GetAffiliateByVerificationCodeAsync(request.VerificationCode, _tenantContext.TenantId);
        return result is null ? null : _mapper.Map<UsersAffiliatesDto>(result);
    }
}
