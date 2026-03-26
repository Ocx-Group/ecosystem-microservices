using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Domain.Constants;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class UpdateCardIdAuthorizationHandler : IRequestHandler<UpdateCardIdAuthorizationCommand, UsersAffiliatesDto?>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public UpdateCardIdAuthorizationHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext, IMapper mapper)
    {
        _repo = repo;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<UsersAffiliatesDto?> Handle(UpdateCardIdAuthorizationCommand request, CancellationToken ct)
    {
        var user = await _repo.GetAffiliateByIdAsync(request.Id, _tenantContext.TenantId);
        if (user is null) return null;

        user.CardIdAuthorization = request.Option == 1;
        user.CardIdMessage = request.Option == 1
            ? nameof(AccountServiceConstants.CardIdStatus.Aprobado)
            : nameof(AccountServiceConstants.CardIdStatus.Pendiente);

        user = await _repo.UpdateAffiliateAsync(user);
        return _mapper.Map<UsersAffiliatesDto>(user);
    }
}
