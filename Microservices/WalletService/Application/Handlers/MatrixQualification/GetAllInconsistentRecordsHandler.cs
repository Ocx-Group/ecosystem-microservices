using Ecosystem.WalletService.Application.Commands.MatrixQualification;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.MatrixQualification;

public class GetAllInconsistentRecordsHandler : IRequestHandler<GetAllInconsistentRecordsCommand, string>
{
    private readonly IMatrixQualificationRepository _matrixQualificationRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GetAllInconsistentRecordsHandler> _logger;

    public GetAllInconsistentRecordsHandler(
        IMatrixQualificationRepository matrixQualificationRepository,
        IWalletRepository walletRepository,
        IWalletRequestRepository walletRequestRepository,
        ITenantContext tenantContext,
        ILogger<GetAllInconsistentRecordsHandler> logger)
    {
        _matrixQualificationRepository = matrixQualificationRepository;
        _walletRepository = walletRepository;
        _walletRequestRepository = walletRequestRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<string> Handle(GetAllInconsistentRecordsCommand request, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.TenantId;
        var inconsistentRecords = await _matrixQualificationRepository.GetAllInconsistentRecordsAsync();
        var correctedCount = 0;
        var records = inconsistentRecords as Domain.Models.MatrixQualification[] ?? inconsistentRecords.ToArray();

        foreach (var record in records)
        {
            try
            {
                var commissions = await _walletRepository.GetQualificationBalanceAsync(record.UserId, brandId);
                var totalWithdrawn = await _walletRequestRepository.GetTotalWithdrawnByAffiliateId(record.UserId);
                var availableBalance = await _walletRepository.GetAvailableBalanceByAffiliateId((int)record.UserId, brandId);
                var amountRequests = await _walletRequestRepository.GetTotalWalletRequestAmountByAffiliateId((int)record.UserId, brandId);
                availableBalance -= amountRequests;

                record.TotalEarnings = commissions ?? 0m;
                record.WithdrawnAmount = totalWithdrawn ?? 0m;
                record.AvailableBalance = availableBalance;
                record.LastQualificationTotalEarnings = record.TotalEarnings;
                record.LastQualificationWithdrawnAmount = record.WithdrawnAmount;
                record.LastQualificationDate = DateTime.Now;
                record.UpdatedAt = DateTime.Now;

                await _matrixQualificationRepository.UpdateAsync(record);
                correctedCount++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fixing qualification record {QualificationId} for user {UserId}",
                    record.QualificationId, record.UserId);
            }
        }

        return $"Operation completed successfully. Corrected {correctedCount} of {records.Length} records.";
    }
}
