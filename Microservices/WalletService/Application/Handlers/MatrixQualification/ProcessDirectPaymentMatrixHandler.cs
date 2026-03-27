using System.Net;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.MatrixQualification;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Requests.MatrixRequest;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Ecosystem.WalletService.Application.Handlers.MatrixQualification;

public class ProcessDirectPaymentMatrixHandler : IRequestHandler<ProcessDirectPaymentMatrixCommand, bool>
{
    private readonly IMatrixQualificationRepository _matrixQualificationRepository;
    private readonly IMatrixEarningsRepository _matrixEarningsRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly IConfigurationAdapter _configurationAdapter;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<ProcessDirectPaymentMatrixHandler> _logger;

    public ProcessDirectPaymentMatrixHandler(
        IMatrixQualificationRepository matrixQualificationRepository,
        IMatrixEarningsRepository matrixEarningsRepository,
        IWalletRepository walletRepository,
        IWalletRequestRepository walletRequestRepository,
        IAccountServiceAdapter accountServiceAdapter,
        IConfigurationAdapter configurationAdapter,
        ITenantContext tenantContext,
        ILogger<ProcessDirectPaymentMatrixHandler> logger)
    {
        _matrixQualificationRepository = matrixQualificationRepository;
        _matrixEarningsRepository = matrixEarningsRepository;
        _walletRepository = walletRepository;
        _walletRequestRepository = walletRequestRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _configurationAdapter = configurationAdapter;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(ProcessDirectPaymentMatrixCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var brandId = _tenantContext.TenantId;
            var targetUserId = request.RecipientId ?? request.UserId;

            var matrixConfigResponse = await _configurationAdapter.GetMatrixConfiguration(brandId, request.MatrixType);
            if (matrixConfigResponse.Content == null || matrixConfigResponse.StatusCode != HttpStatusCode.OK)
                throw new InvalidDataException($"Error retrieving matrix configuration: {matrixConfigResponse.StatusCode}");

            var matrixConfig = JsonConvert.DeserializeObject<MatrixConfigurationResponse>(matrixConfigResponse.Content!)?.Data;
            if (matrixConfig == null) throw new InvalidOperationException("Matrix configuration is invalid");

            var availableBalance = await _walletRepository.GetAvailableBalanceByAffiliateId(request.UserId, brandId);
            var pendingAmount = await _walletRequestRepository.GetTotalWalletRequestAmountByAffiliateId(request.UserId, brandId);
            availableBalance -= pendingAmount;
            if (availableBalance < matrixConfig.FeeAmount) return false;

            var adminBase = Math.Round(matrixConfig.FeeAmount * 0.30m, 2);

            var positionResponse = await _accountServiceAdapter.IsActiveInMatrix(
                new MatrixRequest { UserId = targetUserId, MatrixType = request.MatrixType }, brandId);
            var existing = JsonConvert.DeserializeObject<MatrixPositionResponse>(positionResponse.Content!)?.Data;
            if (positionResponse.IsSuccessful && existing == true) return false;

            var payerInfo = await _accountServiceAdapter.GetUserInfo(request.UserId, brandId);
            var payerName = payerInfo?.UserName ?? "Usuario";
            var targetInfo = await _accountServiceAdapter.GetUserInfo(targetUserId, brandId);
            var targetName = targetInfo?.UserName ?? "Usuario";

            var qualification = await _matrixQualificationRepository.GetByUserAndMatrixTypeAsync(targetUserId, request.MatrixType);
            if (qualification?.IsQualified == true) return false;

            await using var transaction = await _matrixEarningsRepository.BeginTransactionAsync();
            try
            {
                await _walletRepository.CreateWalletAsync(new Domain.Models.Wallet
                {
                    AffiliateId = request.UserId, UserId = 0,
                    Concept = $"Activation of {matrixConfig.MatrixName} for {targetName}",
                    Deferred = 0, Debit = matrixConfig.FeeAmount, Status = true,
                    AffiliateUserName = payerName, AdminUserName = "adminrecycoin",
                    ConceptType = "purchasing_pool", BrandId = brandId, Date = DateTime.Now,
                });

                if (qualification == null)
                {
                    qualification = new Domain.Models.MatrixQualification
                    {
                        UserId = targetUserId, MatrixType = request.MatrixType,
                        TotalEarnings = matrixConfig.Threshold, WithdrawnAmount = 0,
                        AvailableBalance = availableBalance - matrixConfig.FeeAmount,
                        IsQualified = true, QualificationCount = 1,
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
                    qualification.AvailableBalance = availableBalance - matrixConfig.FeeAmount;
                    qualification.LastQualificationTotalEarnings = qualification.TotalEarnings;
                    qualification.LastQualificationWithdrawnAmount = qualification.WithdrawnAmount;
                    qualification.UpdatedAt = DateTime.Now;
                    await _matrixQualificationRepository.UpdateAsync(qualification);
                }

                await _walletRepository.CreateAsync(new Domain.Models.Wallet
                {
                    AffiliateId = 0, UserId = 1,
                    Concept = $"Admin fee 30% - {matrixConfig.MatrixName} (User {targetUserId})",
                    Detail = $"Cycle {qualification.QualificationCount}",
                    Debit = 0, Credit = adminBase,
                    AffiliateUserName = Constants.RecycoinAdmin, AdminUserName = Constants.RecycoinAdmin,
                    Status = true, ConceptType = nameof(WalletConceptType.commission_passed_wallet),
                    BrandId = brandId, Date = DateTime.Now,
                });

                await _accountServiceAdapter.PlaceUserInMatrix(
                    new MatrixRequest { UserId = targetUserId, MatrixType = request.MatrixType }, brandId);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in direct payment matrix activation for user {UserId}", request.UserId);
            return false;
        }
    }
}
