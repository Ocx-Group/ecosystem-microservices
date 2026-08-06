using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Queries.Affiliate;
using Ecosystem.AccountService.Domain.Constants;
using Ecosystem.AccountService.Domain.DTOs.PaginationDto;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class GetAffiliatesPagedHandler : IRequestHandler<GetAffiliatesPagedQuery, PaginationDto<UsersAffiliatesDto>>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public GetAffiliatesPagedHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext, IMapper mapper)
    {
        _repo = repo;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<PaginationDto<UsersAffiliatesDto>> Handle(GetAffiliatesPagedQuery query, CancellationToken ct)
    {
        var brandId = _tenantContext.TenantId;
        var page = await _repo.GetAffiliatesPagedAsync(brandId, query.Request);
        var items = _mapper.Map<List<UsersAffiliatesDto>>(page.Items);

        var relatedIds = items
            .SelectMany(x => new[] { x.Father, x.Sponsor, x.BinarySponsor })
            .Where(id => id != 0)
            .Distinct()
            .Select(id => (long)id)
            .ToArray();

        var userNamesById = relatedIds.Length == 0
            ? new Dictionary<long, string>()
            : (await _repo.GetAffiliatesByIds(relatedIds, brandId))
                .ToDictionary(x => x.Id, x => x.Username);

        foreach (var item in items)
        {
            item.FatherUserName = ResolveUserName(item.Father, userNamesById);
            item.SponsorUserName = ResolveUserName(item.Sponsor, userNamesById);
            item.BinarySponsorUserName = ResolveUserName(item.BinarySponsor, userNamesById);
        }

        return new PaginationDto<UsersAffiliatesDto>
        {
            CurrentPage = page.CurrentPage,
            PageSize = page.PageSize,
            TotalCount = page.TotalCount,
            TotalPages = page.TotalPages,
            Items = items
        };
    }

    private static string? ResolveUserName(int id, IReadOnlyDictionary<long, string> userNamesById)
    {
        if (id == 0) return AccountServiceConstants.Admin;
        return userNamesById.TryGetValue(id, out var userName) ? userName : null;
    }
}
