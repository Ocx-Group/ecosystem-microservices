using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Domain.Constants;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class UpdateCardIdImageHandler : IRequestHandler<UpdateCardIdImageCommand, UsersAffiliatesDto?>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public UpdateCardIdImageHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext, IMapper mapper)
    {
        _repo = repo;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<UsersAffiliatesDto?> Handle(UpdateCardIdImageCommand request, CancellationToken ct)
    {
        var user = await _repo.GetAffiliateByIdAsync(request.Id, _tenantContext.TenantId);
        if (user is null) return null;

        user.StatusActivation = AccountServiceConstants.AffiliateStatus.Aprobación_Pendiente.ToString();
        user.ImagePathId = request.CardIdImage;
        user.CardIdMessage = nameof(AccountServiceConstants.CardIdStatus.Pendiente);
        user = await _repo.UpdateImageAffiliateAsync(user);
        return _mapper.Map<UsersAffiliatesDto>(user);
    }
}
