using AutoMapper;
using Ecosystem.AccountService.Application.Adapters;
using Ecosystem.AccountService.Application.Commands.AffiliateBtc;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Helpers;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.AffiliateBtc;

public class CreateAffiliateBtcHandler : IRequestHandler<CreateAffiliateBtcCommand, List<AffiliateBtcDto>>
{
    private readonly IAffiliateBtcRepository _affiliateBtcRepository;
    private readonly IUserAffiliateInfoRepository _userAffiliateInfoRepository;
    private readonly IBlockchainService _blockchainService;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly IConfigurationServiceAdapter _configurationServiceAdapter;

    public CreateAffiliateBtcHandler(
        IAffiliateBtcRepository affiliateBtcRepository,
        IUserAffiliateInfoRepository userAffiliateInfoRepository,
        IBlockchainService blockchainService,
        ITenantContext tenantContext,
        IMapper mapper,
        IConfigurationServiceAdapter configurationServiceAdapter)
    {
        _affiliateBtcRepository = affiliateBtcRepository;
        _userAffiliateInfoRepository = userAffiliateInfoRepository;
        _blockchainService = blockchainService;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _configurationServiceAdapter = configurationServiceAdapter;
    }

    public async Task<List<AffiliateBtcDto>> Handle(
        CreateAffiliateBtcCommand request, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.TenantId;
        var brandConfiguration = await _configurationServiceAdapter
            .GetBrandConfigurationAsync(brandId, cancellationToken);

        if (brandConfiguration is null)
            return [];

        var userAffiliate = await _userAffiliateInfoRepository.FindAffiliateByIdAsync(request.AffiliateId, brandId);
        if (userAffiliate is null
            || request.VerificationCode != userAffiliate.VerificationCode
            || !PasswordHelper.VerifyPassword(userAffiliate.Password, request.Password))
        {
            return [];
        }

        var createdWallets = new List<AffiliateBtcDto>();

        if (!string.IsNullOrEmpty(request.Trc20Address) &&
            brandConfiguration.BlockchainNetworkId is int networkId &&
            await IsValidConfiguredAddress(request.Trc20Address, networkId))
        {
            var wallet = await CreateWallet(
                request.AffiliateId,
                request.Trc20Address,
                networkId,
                brandId);
            if (wallet is not null) createdWallets.Add(wallet);
        }

        // BSC wallet for all brands
        if (!string.IsNullOrEmpty(request.BscAddress)
            && await _blockchainService.IsValidBscAddress(request.BscAddress))
        {
            var wallet = await CreateWallet(request.AffiliateId, request.BscAddress, 202, brandId);
            if (wallet is not null) createdWallets.Add(wallet);
        }

        return createdWallets;
    }

    private Task<bool> IsValidConfiguredAddress(string address, int networkId) =>
        networkId == 99
            ? _blockchainService.IsValidTrc20Address(address)
            : _blockchainService.IsValidBscAddress(address);

    private async Task<AffiliateBtcDto?> CreateWallet(long affiliateId, string address, int networkId, long brandId)
    {
        var newAffiliateBtc = new AffiliatesBtc
        {
            AffiliateId = affiliateId,
            Address = address,
            BrandId = brandId,
            Status = 1,
            NetworkId = networkId
        };

        try
        {
            var created = await _affiliateBtcRepository.CreateAffiliateBtcAsync(newAffiliateBtc);
            return _mapper.Map<AffiliateBtcDto>(created);
        }
        catch
        {
            return null;
        }
    }
}
