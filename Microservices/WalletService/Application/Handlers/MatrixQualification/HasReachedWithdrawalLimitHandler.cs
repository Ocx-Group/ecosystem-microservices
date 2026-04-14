using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.MatrixQualification;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.MatrixQualification;

public class HasReachedWithdrawalLimitHandler : IRequestHandler<HasReachedWithdrawalLimitQuery, bool>
{
    private readonly IMatrixQualificationRepository _matrixQualificationRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IConfigurationAdapter _configurationAdapter;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<HasReachedWithdrawalLimitHandler> _logger;

    public HasReachedWithdrawalLimitHandler(
        IMatrixQualificationRepository matrixQualificationRepository,
        IWalletRepository walletRepository,
        IConfigurationAdapter configurationAdapter,
        ITenantContext tenantContext,
        ILogger<HasReachedWithdrawalLimitHandler> logger)
    {
        _matrixQualificationRepository = matrixQualificationRepository;
        _walletRepository = walletRepository;
        _configurationAdapter = configurationAdapter;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(HasReachedWithdrawalLimitQuery request, CancellationToken cancellationToken)
    {
        var userId = (int)request.UserId;
        var brandId = _tenantContext.TenantId == 0 ? 2 : _tenantContext.TenantId;

        try
        {
            var totalCommissions = await _walletRepository.GetQualificationBalanceAsync(userId, brandId) ?? 0m;

            var allMatrices = await _configurationAdapter.GetAllMatrixConfigurations(brandId);
            if (allMatrices is null || allMatrices.Count == 0) return false;
            allMatrices = allMatrices.OrderBy(m => m.MatrixType).ToList();

            var nextMatrixType = await GetNextUnqualifiedMatrixTypeAsync(userId, allMatrices);

            var cfg = await _configurationAdapter.GetMatrixConfiguration(brandId, nextMatrixType);
            if (cfg is null) return false;

            var qualification = await _matrixQualificationRepository.GetByUserAndMatrixTypeAsync(userId, nextMatrixType);
            var cycle = qualification?.QualificationCount ?? 0;
            var goal = cycle == 0 ? cfg.Threshold : cfg.RangeMax * cycle;
            var withdrawalLimit = goal * 0.84m;

            var hasReachedLimit = totalCommissions >= withdrawalLimit;
            _logger.LogInformation("User {UserId} withdrawal limit check: Matrix {MatrixType}, Limit: {HasReached}",
                userId, nextMatrixType, hasReachedLimit);

            return hasReachedLimit;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking withdrawal limit for user {UserId}", userId);
            return false;
        }
    }

    private async Task<int> GetNextUnqualifiedMatrixTypeAsync(int userId, List<MatrixConfiguration> allMatrices)
    {
        var qualifications = (await _matrixQualificationRepository.GetAllByUserIdAsync(userId))
            .ToArray();

        if (qualifications.Length == 0) return 1;

        var qualificationDict = qualifications.ToDictionary(q => q.MatrixType);
        var maxCycle = qualifications.Max(q => q.QualificationCount);

        foreach (var matrixType in allMatrices.OrderBy(m => m.MatrixType).Select(m => m.MatrixType))
        {
            if (!qualificationDict.TryGetValue(matrixType, out var q)) return matrixType;
            if (q.QualificationCount < maxCycle) return matrixType;
            if (q.QualificationCount == maxCycle && !q.IsQualified) return matrixType;
        }

        return 1;
    }
}
