using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Helpers;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class UpdateSecretQuestionHandler : IRequestHandler<UpdateSecretQuestionCommand, UsersAffiliatesDto?>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public UpdateSecretQuestionHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext, IMapper mapper)
    {
        _repo = repo;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<UsersAffiliatesDto?> Handle(UpdateSecretQuestionCommand request, CancellationToken ct)
    {
        var user = await _repo.GetAffiliateByIdAsync(request.Id, _tenantContext.TenantId);
        if (user is null) return null;

        if (!PasswordHelper.VerifyPassword(user.Password, request.Password))
            return null;

        user.SecretQuestion = request.SecretQuestion;
        user.SecretAnswer = request.SecretAnswer;
        user = await _repo.UpdateAffiliateAsync(user);
        return _mapper.Map<UsersAffiliatesDto>(user);
    }
}
