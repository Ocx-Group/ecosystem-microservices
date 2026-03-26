using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class GetAccountsEcoPoolHandler : IRequestHandler<GetAccountsEcoPoolCommand, ICollection<UserEcoPoolDto>>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public GetAccountsEcoPoolHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext, IMapper mapper)
    {
        _repo = repo;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<ICollection<UserEcoPoolDto>> Handle(GetAccountsEcoPoolCommand request, CancellationToken ct)
    {
        var users = await _repo.GetAffiliatesByIds(request.AffiliatesIds, _tenantContext.TenantId);
        var usersEcoPool = _mapper.Map<ICollection<UserEcoPoolDto>>(users);

        foreach (var user in usersEcoPool)
            user.FamilyTree = await GetLevelsEcoPool(user, request.Levels);

        return usersEcoPool;
    }

    private async Task<IEnumerable<UserEcoPoolDto>> GetLevelsEcoPool(UserEcoPoolDto userEcoPoolDto, int levels)
    {
        var familyTree = new List<UserEcoPoolDto>();
        var affiliateTemp = userEcoPoolDto;

        for (var i = 0; i < levels; i++)
        {
            var affiliate = await _repo.FindAffiliateByIdAsync((int)affiliateTemp.Father, _tenantContext.TenantId);
            if (affiliate is null)
                return familyTree;

            affiliateTemp = _mapper.Map<UserEcoPoolDto>(affiliate);
            affiliateTemp.Level = i + 1;
            familyTree.Add(affiliateTemp);
        }

        return familyTree;
    }
}
