using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileCommand, UsersAffiliatesDto?>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public UpdateUserProfileHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext, IMapper mapper)
    {
        _repo = repo;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<UsersAffiliatesDto?> Handle(UpdateUserProfileCommand request, CancellationToken ct)
    {
        var user = await _repo.GetAffiliateByIdAsync(request.Id, _tenantContext.TenantId);
        if (user is null) return null;

        user.Identification = request.Identification;
        user.BinaryMatrixSide = request.BinaryMatrixSide;
        user.Address = request.Address ?? user.Address;
        user.Phone = request.Phone;
        user.Zipcode = request.ZipCode ?? user.Zipcode;
        user.Country = request.Country ?? user.Country;
        user.Birthday = request.Birthday ?? user.Birthday;
        user.TaxId = request.TaxId ?? user.TaxId;
        user.BeneficiaryName = request.BeneficiaryName ?? user.BeneficiaryName;
        user.LegalAuthorizedFirst = request.LegalAuthorizedFirst ?? user.LegalAuthorizedFirst;
        user.LegalAuthorizedSecond = request.LegalAuthorizedSecond ?? user.LegalAuthorizedSecond;
        user.BrandId = _tenantContext.TenantId;
        user.BeneficiaryEmail = request.BeneficiaryEmail ?? user.BeneficiaryEmail;
        user.BeneficiaryPhone = request.BeneficiaryPhone ?? user.BeneficiaryPhone;

        user = await _repo.UpdateAffiliateAsync(user);
        return _mapper.Map<UsersAffiliatesDto>(user);
    }
}
