using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class UpdateAffiliateImageHandler : IRequestHandler<UpdateAffiliateImageCommand, UsersAffiliatesDto?>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public UpdateAffiliateImageHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext, IMapper mapper)
    {
        _repo = repo;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<UsersAffiliatesDto?> Handle(UpdateAffiliateImageCommand request, CancellationToken ct)
    {
        var user = await _repo.GetAffiliateByIdAsync(request.Id, _tenantContext.TenantId);
        if (user is null) return null;

        user.ImageProfileUrl = request.ImageProfileUrl;
        user = await _repo.UpdateImageAffiliateAsync(user);
        return _mapper.Map<UsersAffiliatesDto>(user);
    }
}
