using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Queries.Affiliate;
using Ecosystem.AccountService.Domain.Constants;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class GetAffiliateByIdHandler : IRequestHandler<GetAffiliateByIdQuery, UsersAffiliatesDto?>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public GetAffiliateByIdHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext, IMapper mapper)
    {
        _repo = repo;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<UsersAffiliatesDto?> Handle(GetAffiliateByIdQuery request, CancellationToken ct)
    {
        var affiliate = await _repo.GetAffiliateByIdAsync(request.Id, _tenantContext.TenantId);
        if (affiliate is null)
            return null;

        var dto = _mapper.Map<UsersAffiliatesDto>(affiliate);

        // Resolve binary sponsor father
        if (dto.BinarySponsor is 0)
        {
            dto.FatherUser = new UsersAffiliatesDto { UserName = AccountServiceConstants.Admin };
        }
        else
        {
            var father = await _repo.GetAffiliateByIdAsync(dto.BinarySponsor, _tenantContext.TenantId);
            if (father is not null)
                dto.FatherUser = new UsersAffiliatesDto { UserName = father.Username };
        }

        // Resolve unilevel father
        if (dto.Father is 0)
        {
            dto.FatherUserUniLevel = new UsersAffiliatesDto { UserName = AccountServiceConstants.Admin };
        }
        else
        {
            var father = await _repo.GetAffiliateByIdAsync(dto.Father, _tenantContext.TenantId);
            if (father is not null)
                dto.FatherUserUniLevel = new UsersAffiliatesDto { UserName = father.Username };
        }

        return dto;
    }
}
