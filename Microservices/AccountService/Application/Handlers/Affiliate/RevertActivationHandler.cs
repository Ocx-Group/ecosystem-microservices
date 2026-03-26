using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class RevertActivationHandler : IRequestHandler<RevertActivationCommand, UsersAffiliatesDto?>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public RevertActivationHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext, IMapper mapper)
    {
        _repo = repo;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<UsersAffiliatesDto?> Handle(RevertActivationCommand request, CancellationToken ct)
    {
        var user = await _repo.GetAffiliateByIdAsync(request.Id, _tenantContext.TenantId);
        if (user is null) return null;

        user.ActivationDate = null;
        user.ExternalGradingId = 0;
        user.ExternalProductId = 0;
        user = await _repo.UpdateAffiliateAsync(user);
        return _mapper.Map<UsersAffiliatesDto>(user);
    }
}
