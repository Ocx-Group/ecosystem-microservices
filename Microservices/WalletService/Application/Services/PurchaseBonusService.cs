using Ecosystem.Domain.Core.BrandConfiguration;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.Services;
using Microsoft.Extensions.Logging;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;

namespace Ecosystem.WalletService.Application.Services;

/// <summary>
/// Distributes the per-purchase bonus ("bono diario") to the buyer's upline.
///
/// The percentages, the master switch and whether the bonus applies to every purchase
/// all come from the brand's own configuration, so no brand or payment method is
/// special-cased here. Previously each payment strategy carried its own hardcoded
/// level array and a <c>BrandId</c> check.
/// </summary>
public class PurchaseBonusService : IPurchaseBonusService
{
    private readonly IConfigurationAdapter _configurationAdapter;
    private readonly IWalletRepository _walletRepository;
    private readonly ILogger<PurchaseBonusService> _logger;

    public PurchaseBonusService(
        IConfigurationAdapter configurationAdapter,
        IWalletRepository walletRepository,
        ILogger<PurchaseBonusService> logger)
    {
        _configurationAdapter = configurationAdapter;
        _walletRepository = walletRepository;
        _logger = logger;
    }

    public async Task DistributeAsync(WalletRequestModel request, decimal invoiceAmount)
    {
        BrandConfigurationDto? config = null;

        try
        {
            config = await _configurationAdapter.GetBrandConfiguration(request.BrandId);

            if (config is not { CommissionEnabled: true, CommissionLevels.Length: > 0 })
                return;

            // A brand can either pay the bonus on every purchase or only on the ones an
            // operator explicitly opted in from the purchase screen.
            if (!config.DailyBonusAlwaysDistribute && request.DailyBonusActivation != true)
                return;

            if (invoiceAmount <= 0)
                return;

            var beneficiaryIds = await _walletRepository.DistributeCommissionsPerPurchaseAsync(
                new DistributeCommissionsRequest
                {
                    AffiliateId = request.AffiliateId,
                    InvoiceAmount = invoiceAmount,
                    BrandId = request.BrandId,
                    AdminUserName = config.AdminUserName,
                    LevelPercentages = config.CommissionLevels
                });

            // No beneficiaries is legitimate: the buyer sits at the root of the tree and
            // the whole percentage stays with the company. It is logged apart from a
            // successful payout so it cannot be mistaken for one.
            if (beneficiaryIds.Count == 0)
            {
                _logger.LogInformation(
                    "Purchase bonus for affiliate {AffiliateId} of brand {BrandId} reached no upline; "
                    + "nothing was credited",
                    request.AffiliateId,
                    request.BrandId);
                return;
            }

            _logger.LogInformation(
                "Purchase bonus distributed for affiliate {AffiliateId} of brand {BrandId} over {LevelCount} "
                + "levels, reaching {BeneficiaryCount} beneficiaries",
                request.AffiliateId,
                request.BrandId,
                config.CommissionLevels.Length,
                beneficiaryIds.Count);
        }
        catch (Exception ex)
        {
            // The purchase itself already succeeded and was debited. Failing here must
            // not roll it back, so the error is recorded for reprocessing instead. The
            // database function is atomic, so nothing was credited partially.
            _logger.LogError(
                ex,
                "Error distributing the purchase bonus for affiliate {AffiliateId} of brand {BrandId} "
                + "over levels [{Levels}]. The purchase itself was not affected",
                request.AffiliateId,
                request.BrandId,
                string.Join(", ", config?.CommissionLevels ?? []));
        }
    }
}
