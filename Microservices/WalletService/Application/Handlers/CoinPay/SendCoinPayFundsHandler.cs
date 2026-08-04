using System.Net;
using Ecosystem.Domain.Core.Caching;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.CoinPay;
using Ecosystem.WalletService.Application.Extensions;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.DTOs.CoinPayDto;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Requests.CoinPayRequest;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
// Disambiguates from the Ecosystem.WalletService.Application.*.Wallet namespaces.
using WalletModel = Ecosystem.WalletService.Domain.Models.Wallet;

namespace Ecosystem.WalletService.Application.Handlers.CoinPay;

public class SendCoinPayFundsHandler : IRequestHandler<SendCoinPayFundsCommand, SendFundsDto>
{
    private readonly ICoinPayAdapter _coinPayAdapter;
    private readonly IAccountServiceAdapter _accountAdapter;
    private readonly IConfigurationAdapter _configurationAdapter;
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletWithDrawalRepository _walletWithdrawalRepository;
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly ICacheService _cacheService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<SendCoinPayFundsHandler> _logger;

    public SendCoinPayFundsHandler(
        ICoinPayAdapter coinPayAdapter,
        IAccountServiceAdapter accountAdapter,
        IConfigurationAdapter configurationAdapter,
        IWalletRepository walletRepository,
        IWalletWithDrawalRepository walletWithdrawalRepository,
        IWalletRequestRepository walletRequestRepository,
        ICacheService cacheService,
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext,
        ILogger<SendCoinPayFundsHandler> logger)
    {
        _coinPayAdapter = coinPayAdapter;
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

    public async Task<SendFundsDto> Handle(SendCoinPayFundsCommand command, CancellationToken cancellationToken)
    {
        var result = new SendFundsDto();

        if (command.Requests.Length == 0)
            return result;

        var brandId = _tenantContext.TenantId;
        var brandConfig = await _configurationAdapter.GetBrandConfiguration(brandId, cancellationToken);
        var successfulIds = new List<long>();

        foreach (var request in command.Requests)
        {
            try
            {
                var response = await ProcessWithdrawal(request, brandId, brandConfig?.AdminUserName);

                if (response.StatusCode == Constants.SuccessStatusCode)
                {
                    result.SuccessfulResponses.Add(response);
                    successfulIds.Add(request.Id);
                }
                else
                {
                    result.FailedResponses.Add(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Withdrawal {RequestId} could not be processed", request.Id);
                result.FailedResponses.Add(new SendFundsResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "Failed to send funds"
                });
            }
        }

        if (successfulIds.Count > 0)
            await MarkWithdrawalsCompleted(successfulIds);

        return result;
    }

    private async Task<SendFundsResponse> ProcessWithdrawal(
        WithDrawalRequest request, long brandId, string? adminUserName)
    {
        var user = await _accountAdapter.GetUserInfo(request.AffiliateId, brandId);
        if (user?.UserName is null)
        {
            _logger.LogWarning("No account found for affiliate {AffiliateId}", request.AffiliateId);
            return new SendFundsResponse { StatusCode = (int)HttpStatusCode.NotFound, Message = "User not found" };
        }

        var addresses = await _accountAdapter.GetAffiliateBtcByAffiliateId(request.AffiliateId, brandId);
        var payoutAddress = addresses?.FirstOrDefault(a => !string.IsNullOrWhiteSpace(a.Address));

        if (payoutAddress is null)
        {
            _logger.LogWarning("No payout address on file for affiliate {AffiliateId}", request.AffiliateId);
            return new SendFundsResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "No valid address found"
            };
        }

        var sendFundsRequest = new SendFundRequest
        {
            IdCurrency = Constants.UsdtIdCurrency,
            IdNetwork = brandId == 1 ? Constants.UsdtIdNetwork : Constants.BnbIdNetwork,
            Address = payoutAddress.Address,
            Amount = request.Amount,
            Details = Constants.SendFundsConcept,
            AmountPlusFee = false
        };

        var response = await _coinPayAdapter.SendFunds(sendFundsRequest);

        if (response is not { StatusCode: Constants.SuccessStatusCode })
        {
            _logger.LogWarning(
                "CoinPay refused the withdrawal for affiliate {AffiliateId} with status {StatusCode}",
                request.AffiliateId, response?.StatusCode);

            return response ?? new SendFundsResponse
            {
                StatusCode = (int)HttpStatusCode.BadGateway,
                Message = "Failed to send funds"
            };
        }

        // Funds have already left CoinPay, so the local bookkeeping must not be skipped:
        // it is committed as one unit and rethrown loudly if it fails.
        await RecordWithdrawal(request, user, sendFundsRequest, response, brandId, adminUserName);

        return response;
    }

    private async Task RecordWithdrawal(
        WithDrawalRequest request,
        UserInfoResponse user,
        SendFundRequest sendFundsRequest,
        SendFundsResponse response,
        long brandId,
        string? adminUserName)
    {
        var now = DateTime.UtcNow;

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Sequential writes: both repositories share the scoped DbContext and cannot
            // be awaited concurrently.
            await _walletRepository.CreateWalletAsync(new WalletModel
            {
                AffiliateId = request.AffiliateId,
                UserId = 1,
                Credit = Constants.EmptyValue,
                Debit = request.Amount,
                Deferred = Constants.EmptyValue,
                Status = true,
                Concept = Constants.SendFundsConcept,
                Support = 1,
                Date = now,
                Compression = true,
                Detail = CoinPayConstants.WithdrawalDetail,
                CreatedAt = now,
                UpdatedAt = now,
                AffiliateUserName = user.UserName,
                AdminUserName = adminUserName,
                ConceptType = nameof(WalletConceptType.balance_transfer),
                BrandId = brandId
            });

            await _walletWithdrawalRepository.CreateWalletWithdrawalAsync(new WalletsWithdrawal
            {
                AffiliateId = request.AffiliateId,
                AffiliateUserName = user.UserName,
                Amount = sendFundsRequest.Amount,
                IsProcessed = true,
                Observation = $"{Constants.SendFundsConcept} {request.Id}",
                AdminObservation = $"{response.Data?.IdTransaction} - {response.Data?.Address ?? "No Address"}",
                Date = now,
                ResponseDate = now,
                RetentionPercentage = Constants.EmptyValue,
                Status = true,
                CreatedAt = now,
                UpdatedAt = now
            });

            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }

        await _cacheService.InvalidateBalanceAsync(request.AffiliateId);
    }

    private async Task MarkWithdrawalsCompleted(List<long> successfulIds)
    {
        var withdrawals = await _walletRequestRepository.GetWalletRequestsByIds(successfulIds);

        foreach (var withdrawal in withdrawals)
        {
            withdrawal.Status = (short)WithdrawalStatus.Completed;
            withdrawal.UpdatedAt = DateTime.UtcNow;
        }

        await _walletRequestRepository.UpdateBulkWalletRequestsAsync(withdrawals);

        _logger.LogInformation("Marked {Count} withdrawal requests as completed", withdrawals.Count);
    }
}
