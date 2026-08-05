using Ecosystem.Domain.Core.Caching;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.CoinPayments;
using Ecosystem.WalletService.Application.Extensions;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
// Disambiguates from the Ecosystem.WalletService.Application.*.Wallet namespaces.
using WalletModel = Ecosystem.WalletService.Domain.Models.Wallet;

namespace Ecosystem.WalletService.Application.Handlers.CoinPayments;

public class CreateMassWithdrawalHandler : IRequestHandler<CreateMassWithdrawalCommand, CoinPaymentWithdrawalResponse?>
{
    /// <summary>A request paired with the payout address resolved for its affiliate.</summary>
    private sealed record PayableWithdrawal(CoinPaymentsWithdrawalRequest Request, string Address);

    private readonly ICoinPaymentsAdapter _adapter;
    private readonly IAccountServiceAdapter _accountAdapter;
    private readonly IConfigurationAdapter _configurationAdapter;
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletWithDrawalRepository _walletWithdrawalRepository;
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly ICacheService _cacheService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<CreateMassWithdrawalHandler> _logger;

    public CreateMassWithdrawalHandler(
        ICoinPaymentsAdapter adapter,
        IAccountServiceAdapter accountAdapter,
        IConfigurationAdapter configurationAdapter,
        IWalletRepository walletRepository,
        IWalletWithDrawalRepository walletWithdrawalRepository,
        IWalletRequestRepository walletRequestRepository,
        ICacheService cacheService,
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext,
        ILogger<CreateMassWithdrawalHandler> logger)
    {
        _adapter = adapter;
        _accountAdapter = accountAdapter;
        _configurationAdapter = configurationAdapter;
        _walletRepository = walletRepository;
        _walletWithdrawalRepository = walletWithdrawalRepository;
        _walletRequestRepository = walletRequestRepository;
        _cacheService = cacheService;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<CoinPaymentWithdrawalResponse?> Handle(
        CreateMassWithdrawalCommand command, CancellationToken cancellationToken)
    {
        if (command.Requests.Count == 0)
            return null;

        var brandId = _tenantContext.TenantId;
        var payable = await ResolvePayableWithdrawals(command.Requests, brandId);

        if (payable.Count == 0)
        {
            _logger.LogWarning("None of the {Count} withdrawal requests could be paid", command.Requests.Count);
            return null;
        }

        var response = await _adapter.CreateMassWithdrawal(payable.Select(p => new CoinPaymentMassWithdrawalRequest
        {
            Amount = p.Request.Amount - Constants.CoinPaymentTax,
            Address = p.Address,
            Currency = Constants.CoinPaymentsBnbCurrency
        }));

        if (response?.Result is null)
        {
            _logger.LogWarning("CoinPayments refused the mass withdrawal: {Error}", response?.Error);
            return response;
        }

        var brandConfig = await _configurationAdapter.GetBrandConfiguration(brandId, cancellationToken);

        await RecordSuccessfulWithdrawals(payable, response, brandId, brandConfig?.AdminUserName);

        return response;
    }

    /// <summary>
    /// Keeps only the requests that can actually be paid, each carrying its own address.
    /// Pairing them here — rather than by index against the original list, as the old service
    /// did — is what stops a skipped request from shifting every later payout onto the wrong
    /// affiliate.
    /// </summary>
    private async Task<List<PayableWithdrawal>> ResolvePayableWithdrawals(
        IReadOnlyList<CoinPaymentsWithdrawalRequest> requests, long brandId)
    {
        var payable = new List<PayableWithdrawal>();

        foreach (var request in requests)
        {
            var user = await _accountAdapter.GetUserInfo(request.AffiliateId, brandId);
            if (user?.UserName is null)
            {
                _logger.LogWarning("No account found for affiliate {AffiliateId}", request.AffiliateId);
                continue;
            }

            if (await _walletRequestRepository.GetByIdAsync((int)request.Id) is null)
            {
                _logger.LogWarning("Withdrawal request {RequestId} no longer exists", request.Id);
                continue;
            }

            var addresses = await _accountAdapter.GetAffiliateBtcByAffiliateId(request.AffiliateId, brandId);
            var address = addresses?.FirstOrDefault(a => !string.IsNullOrWhiteSpace(a.Address))?.Address;

            if (string.IsNullOrWhiteSpace(address))
            {
                _logger.LogWarning("No payout address on file for affiliate {AffiliateId}", request.AffiliateId);
                continue;
            }

            payable.Add(new PayableWithdrawal(request, address));
        }

        return payable;
    }

    private async Task RecordSuccessfulWithdrawals(
        List<PayableWithdrawal> payable,
        CoinPaymentWithdrawalResponse response,
        long brandId,
        string? adminUserName)
    {
        foreach (var (key, info) in response.Result!)
        {
            if (!string.Equals(info.Error, CoinPaymentsConstants.SuccessError, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("CoinPayments rejected withdrawal {Key}: {Error}", key, info.Error);
                continue;
            }

            // Keys are wd1..wdN in the order the adapter enumerated the payable list.
            var index = ParseWithdrawalIndex(key);
            if (index is null || index.Value >= payable.Count)
            {
                _logger.LogError(
                    "CoinPayments returned the unexpected withdrawal key {Key}; it will not be recorded", key);
                continue;
            }

            var (request, address) = payable[index.Value];

            try
            {
                await RecordWithdrawal(request, address, brandId, adminUserName);
            }
            catch (Exception ex)
            {
                // The funds already left CoinPayments, so this needs manual reconciliation:
                // log loudly and keep processing the remaining payouts.
                _logger.LogError(
                    ex, "Withdrawal {RequestId} was paid but could not be recorded", request.Id);
            }
        }
    }

    private async Task RecordWithdrawal(
        CoinPaymentsWithdrawalRequest request, string address, long brandId, string? adminUserName)
    {
        var user = await _accountAdapter.GetUserInfo(request.AffiliateId, brandId);
        var userName = user?.UserName ?? "unknown";
        var now = DateTime.UtcNow;

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Sequential writes: both repositories share the scoped DbContext and cannot be
            // awaited concurrently.
            await _walletRepository.CreateWalletAsync(new WalletModel
            {
                AffiliateId = request.AffiliateId,
                UserId = Constants.AdminUserId,
                Credit = Constants.EmptyValue,
                // The full amount is debited; the flat tax is only withheld from what is sent out.
                Debit = request.Amount,
                Deferred = Constants.EmptyValue,
                Status = true,
                Concept = Constants.SendFundsConcept,
                Support = 1,
                Date = now,
                Compression = true,
                Detail = CoinPaymentsConstants.WithdrawalDetail,
                CreatedAt = now,
                UpdatedAt = now,
                AffiliateUserName = userName,
                AdminUserName = adminUserName,
                ConceptType = nameof(WalletConceptType.balance_transfer),
                BrandId = brandId
            });

            await _walletWithdrawalRepository.CreateWalletWithdrawalAsync(new WalletsWithdrawal
            {
                AffiliateId = request.AffiliateId,
                AffiliateUserName = userName,
                Amount = request.Amount,
                IsProcessed = true,
                Observation = $"{Constants.SendFundsConcept} {request.Id}",
                AdminObservation = $"MassWithdrawal - ConPayments - {address}",
                Date = now,
                ResponseDate = now,
                RetentionPercentage = Constants.EmptyValue,
                Status = true,
                CreatedAt = now,
                UpdatedAt = now
            });

            var walletRequest = await _walletRequestRepository.GetByIdAsync((int)request.Id);
            if (walletRequest is not null)
            {
                walletRequest.Status = (short)WithdrawalStatus.Completed;
                walletRequest.UpdatedAt = now;
                await _walletRequestRepository.UpdateWalletRequestsAsync(walletRequest);
            }

            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }

        await _cacheService.InvalidateBalanceAsync(request.AffiliateId);
    }

    private static int? ParseWithdrawalIndex(string key)
        => key.StartsWith("wd", StringComparison.OrdinalIgnoreCase)
           && int.TryParse(key.AsSpan(2), out var number)
           && number > 0
            ? number - 1
            : null;
}
