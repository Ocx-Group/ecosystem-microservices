using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.MatrixQualification;
using Ecosystem.WalletService.Domain.Extensions;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Requests.MatrixRequest;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.MatrixQualification;

public class CoinPaymentsMatrixActivationHandler : IRequestHandler<CoinPaymentsMatrixActivationCommand, bool>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMatrixQualificationRepository _matrixQualificationRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly IConfigurationAdapter _configurationAdapter;
    private readonly IMatrixEarningsRepository _matrixEarningsRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<CoinPaymentsMatrixActivationHandler> _logger;

    public CoinPaymentsMatrixActivationHandler(
        ITransactionRepository transactionRepository,
        IMatrixQualificationRepository matrixQualificationRepository,
        IWalletRepository walletRepository,
        IWalletRequestRepository walletRequestRepository,
        IAccountServiceAdapter accountServiceAdapter,
        IConfigurationAdapter configurationAdapter,
        IMatrixEarningsRepository matrixEarningsRepository,
        ITenantContext tenantContext,
        ILogger<CoinPaymentsMatrixActivationHandler> logger)
    {
        _transactionRepository = transactionRepository;
        _matrixQualificationRepository = matrixQualificationRepository;
        _walletRepository = walletRepository;
        _walletRequestRepository = walletRequestRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _configurationAdapter = configurationAdapter;
        _matrixEarningsRepository = matrixEarningsRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(CoinPaymentsMatrixActivationCommand request, CancellationToken cancellationToken)
    {
        var ipn = request.Request;

        if (string.IsNullOrEmpty(ipn.ipn_mode) || ipn.ipn_mode.ToLower() != "hmac") return false;
        if (!request.Headers.TryGetValue("Hmac", out var receivedHmac) || string.IsNullOrEmpty(receivedHmac)) return false;
        if (string.IsNullOrEmpty(ipn.merchant)) return false;
        if (ipn.ipn_type != "api") return false;
        var validCurrencies = new[] { "USDT.TRC20", "USDT.BEP20" };
        if (!validCurrencies.Contains(ipn.currency1)) return false;

        var transactionResult = await _transactionRepository.GetTransactionByTxnId(ipn.txn_id);
        if (transactionResult is null) return false;
        if (transactionResult.Status == 100) return false;

        transactionResult.Status = ipn.status;
        transactionResult.AmountReceived = ipn.received_amount;
        var matrixType = CommonExtensions.ExtractProductIdFromJson_JArray(transactionResult.Products);

        if (ipn.status == -1)
        {
            transactionResult.Acredited = false;
            await _transactionRepository.UpdateTransactionAsync(transactionResult);
            return false;
        }

        if (!transactionResult.Acredited && ipn.status == 100)
        {
            try
            {
                var brandId = _tenantContext.TenantId;
                var activationResult = await ProcessCoinPaymentsMatrixActivationAsync(
                    transactionResult.AffiliateId, matrixType, brandId);

                transactionResult.Acredited = activationResult;
                if (activationResult)
                    _logger.LogInformation("Matrix {MatrixType} activated for user {AffiliateId}", matrixType, transactionResult.AffiliateId);
                else
                    _logger.LogWarning("Failed to activate matrix {MatrixType} for user {AffiliateId}", matrixType, transactionResult.AffiliateId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating matrix for CoinPayments transaction {TxnId}", ipn.txn_id);
                transactionResult.Acredited = false;
            }
        }

        await _transactionRepository.UpdateTransactionAsync(transactionResult);
        return true;
    }

    private async Task<bool> ProcessCoinPaymentsMatrixActivationAsync(int userId, int matrixType, long brandId)
    {
        var matrixConfig = await _configurationAdapter.GetMatrixConfiguration(brandId, matrixType);
        if (matrixConfig is null) return false;

        var isActive = await _accountServiceAdapter.IsActiveInMatrix(
            new MatrixRequest { UserId = userId, MatrixType = matrixType }, brandId);
        if (isActive) return false;

        var qualification = await _matrixQualificationRepository.GetByUserAndMatrixTypeAsync(userId, matrixType);
        if (qualification?.IsQualified == true) return false;

        var availableBalance = await _walletRepository.GetAvailableBalanceByAffiliateId(userId, brandId);
        var pendingAmount = await _walletRequestRepository.GetTotalWalletRequestAmountByAffiliateId(userId, brandId);
        availableBalance -= pendingAmount;

        if (qualification == null)
        {
            qualification = new Domain.Models.MatrixQualification
            {
                UserId = userId, MatrixType = matrixType,
                TotalEarnings = matrixConfig.Threshold, WithdrawnAmount = 0,
                AvailableBalance = availableBalance, IsQualified = true, QualificationCount = 1,
                LastQualificationTotalEarnings = matrixConfig.Threshold,
                LastQualificationWithdrawnAmount = 0,
                CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, LastQualificationDate = DateTime.Now
            };
            await _matrixQualificationRepository.CreateAsync(qualification);
        }
        else
        {
            qualification.IsQualified = true;
            qualification.QualificationCount += 1;
            qualification.AvailableBalance = availableBalance;
            qualification.LastQualificationTotalEarnings = qualification.TotalEarnings;
            qualification.LastQualificationWithdrawnAmount = qualification.WithdrawnAmount;
            qualification.LastQualificationDate = DateTime.Now;
            qualification.UpdatedAt = DateTime.Now;
            await _matrixQualificationRepository.UpdateAsync(qualification);
        }

        await _accountServiceAdapter.PlaceUserInMatrix(
            new MatrixRequest { UserId = userId, MatrixType = matrixType }, brandId);

        return true;
    }
}
