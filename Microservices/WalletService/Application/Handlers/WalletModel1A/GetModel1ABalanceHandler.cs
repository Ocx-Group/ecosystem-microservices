using Ecosystem.Domain.Core.Caching;
using Ecosystem.WalletService.Application.Queries.WalletModel1A;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.DTOs.WalletModel1ADto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletModel1A;

public class GetModel1ABalanceHandler : IRequestHandler<GetModel1ABalanceQuery, BalanceInformationModel1ADto>
{
    private readonly IWalletModel1ARepository _walletModel1ARepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetModel1ABalanceHandler> _logger;

    public GetModel1ABalanceHandler(
        IWalletModel1ARepository walletModel1ARepository,
        ICacheService cacheService,
        ILogger<GetModel1ABalanceHandler> logger)
    {
        _walletModel1ARepository = walletModel1ARepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<BalanceInformationModel1ADto> Handle(GetModel1ABalanceQuery request, CancellationToken cancellationToken)
    {
        var affiliateId = request.AffiliateId;
        var key = string.Format(CacheKeys.BalanceInformationModel1A, affiliateId);
        var existsKey = await _cacheService.KeyExists(key);

        BalanceInformationModel1ADto response;

        if (!existsKey)
        {
            var availableBalance = await _walletModel1ARepository.GetAvailableBalanceByAffiliateId(affiliateId);
            var totalAcquisitions = await _walletModel1ARepository.GetTotalAcquisitionsByAffiliateId(affiliateId);
            var reverseBalance = await _walletModel1ARepository.GetReverseBalanceByAffiliateId(affiliateId);
            var serviceBalance = await _walletModel1ARepository.GetTotalServiceBalance(affiliateId);
            var totalCommissionsPaid = await _walletModel1ARepository.GetTotalCommissionsPaidBalance(affiliateId);

            response = new BalanceInformationModel1ADto
            {
                AvailableBalance = availableBalance,
                ReverseBalance = reverseBalance ?? 0,
                TotalAcquisitions = totalAcquisitions ?? 0,
                TotalCommissionsPaid = totalCommissionsPaid ?? 0,
                ServiceBalance = serviceBalance ?? 0
            };

            if (response.ReverseBalance != 0m)
                response.AvailableBalance -= response.ReverseBalance;

            await _cacheService.Set(key, response, TimeSpan.FromHours(1));
            return response;
        }

        response = await _cacheService.Get<BalanceInformationModel1ADto>(key) ?? new BalanceInformationModel1ADto();
        return response;
    }
}
