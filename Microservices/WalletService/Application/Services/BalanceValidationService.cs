using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Services;

public class BalanceValidationService : IBalanceValidationService
{
    private readonly IWalletRepository _walletRepository;
    private readonly ILogger<BalanceValidationService> _logger;

    public BalanceValidationService(
        IWalletRepository walletRepository,
        ILogger<BalanceValidationService> logger)
    {
        _walletRepository = walletRepository;
        _logger = logger;
    }

    public async Task<BalanceValidationResult> ValidateBalance(
        int affiliateId,
        long brandId,
        decimal requiredAmount)
    {
        var availableBalance = await _walletRepository.GetAvailableBalanceByAffiliateId(affiliateId, brandId);

        if (requiredAmount > availableBalance)
        {
            _logger.LogWarning(
                "Insufficient balance for affiliate {AffiliateId}: required {Required}, available {Available}",
                affiliateId, requiredAmount, availableBalance);
            return BalanceValidationResult.InsufficientFunds(availableBalance);
        }

        return BalanceValidationResult.Success(availableBalance);
    }
}
