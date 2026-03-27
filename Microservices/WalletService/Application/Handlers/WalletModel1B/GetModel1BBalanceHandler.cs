using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.WalletModel1B;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.DTOs.WalletModel1BDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletModel1B;

public class GetModel1BBalanceHandler : IRequestHandler<GetModel1BBalanceQuery, BalanceInformationModel1BDto>
{
    private readonly IWalletModel1BRepository _walletModel1BRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetModel1BBalanceHandler> _logger;

    public GetModel1BBalanceHandler(
        IWalletModel1BRepository walletModel1BRepository,
        ICacheService cacheService,
        ILogger<GetModel1BBalanceHandler> logger)
    {
        _walletModel1BRepository = walletModel1BRepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<BalanceInformationModel1BDto> Handle(GetModel1BBalanceQuery request, CancellationToken cancellationToken)
    {
        var affiliateId = request.AffiliateId;
        var key = string.Format(CacheKeys.BalanceInformationModel1B, affiliateId);
        var existsKey = await _cacheService.KeyExists(key);

        BalanceInformationModel1BDto response;

        if (!existsKey)
        {
            var availableBalance = await _walletModel1BRepository.GetAvailableBalanceByAffiliateId(affiliateId);
            var totalAcquisitions = await _walletModel1BRepository.GetTotalAcquisitionsByAffiliateId(affiliateId);
            var reverseBalance = await _walletModel1BRepository.GetReverseBalanceByAffiliateId(affiliateId);
            var serviceBalance = await _walletModel1BRepository.GetTotalServiceBalance(affiliateId);
            var totalCommissionsPaid = await _walletModel1BRepository.GetTotalCommissionsPaidBalance(affiliateId);

            response = new BalanceInformationModel1BDto
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

        response = await _cacheService.Get<BalanceInformationModel1BDto>(key) ?? new BalanceInformationModel1BDto();
        return response;
    }
}
