using Ecosystem.Domain.Core.Caching;
using Ecosystem.WalletService.Application.Queries.Wallet;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.DTOs.BalanceInformationDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class GetBalanceInformationHandler : IRequestHandler<GetBalanceInformationQuery, BalanceInformationDto>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly IBonusRepository _bonusRepository;
    private readonly ICacheService _cacheService;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GetBalanceInformationHandler> _logger;

    public GetBalanceInformationHandler(
        IWalletRepository walletRepository,
        IWalletRequestRepository walletRequestRepository,
        IBonusRepository bonusRepository,
        ICacheService cacheService,
        ITenantContext tenantContext,
        ILogger<GetBalanceInformationHandler> logger)
    {
        _walletRepository = walletRepository;
        _walletRequestRepository = walletRequestRepository;
        _bonusRepository = bonusRepository;
        _cacheService = cacheService;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<BalanceInformationDto> Handle(GetBalanceInformationQuery request, CancellationToken cancellationToken)
    {
        var affiliateId = request.AffiliateId;
        var brandId = _tenantContext.TenantId;
        var key = string.Format(CacheKeys.BalanceInformationModel2, affiliateId);
        var existsKey = await _cacheService.KeyExists(key);

        BalanceInformationDto response;

        if (!existsKey)
        {
            var amountRequests = await _walletRequestRepository.GetTotalWalletRequestAmountByAffiliateId(affiliateId, brandId);
            var availableBalance = await _walletRepository.GetAvailableBalanceByAffiliateId(affiliateId, brandId);
            var reverseBalance = await _walletRepository.GetReverseBalanceByAffiliateId(affiliateId, brandId);
            var totalAcquisitions = await _walletRepository.GetTotalAcquisitionsByAffiliateId(affiliateId, brandId);
            var totalCommissionsPaid = await _walletRepository.GetTotalCommissionsPaid(affiliateId, brandId);
            var totalServiceBalance = await _walletRepository.GetTotalServiceBalance(affiliateId, brandId);
            var bonusAmount = await _bonusRepository.GetBonusAmountByAffiliateId(affiliateId);

            response = new BalanceInformationDto
            {
                AvailableBalance = availableBalance,
                ReverseBalance = reverseBalance ?? 0,
                TotalAcquisitions = Math.Round(totalAcquisitions ?? 0, 2),
                TotalCommissionsPaid = totalCommissionsPaid ?? 0,
                ServiceBalance = totalServiceBalance ?? 0,
                BonusAmount = bonusAmount,
            };

            if (amountRequests != 0m || response.ReverseBalance != 0m)
            {
                response.AvailableBalance -= amountRequests;
                response.AvailableBalance -= response.ReverseBalance;
            }

            await _cacheService.Set(key, response, TimeSpan.FromHours(1));
            return response;
        }

        response = await _cacheService.Get<BalanceInformationDto>(key) ?? new BalanceInformationDto();
        return response;
    }
}
