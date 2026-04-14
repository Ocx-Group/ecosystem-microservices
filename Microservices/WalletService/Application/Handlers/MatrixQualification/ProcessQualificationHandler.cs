using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.MatrixQualification;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Requests.MatrixRequest;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.MatrixQualification;

public class ProcessQualificationHandler : IRequestHandler<ProcessQualificationQuery, (bool AnyQualified, List<int> QualifiedMatrixTypes)>
{
    private readonly IMatrixQualificationRepository _matrixQualificationRepository;
    private readonly IMatrixEarningsRepository _matrixEarningsRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly IConfigurationAdapter _configurationAdapter;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<ProcessQualificationHandler> _logger;

    public ProcessQualificationHandler(
        IMatrixQualificationRepository matrixQualificationRepository,
        IMatrixEarningsRepository matrixEarningsRepository,
        IWalletRepository walletRepository,
        IWalletRequestRepository walletRequestRepository,
        IAccountServiceAdapter accountServiceAdapter,
        IConfigurationAdapter configurationAdapter,
        ITenantContext tenantContext,
        ILogger<ProcessQualificationHandler> logger)
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

    public async Task<(bool AnyQualified, List<int> QualifiedMatrixTypes)> Handle(
        ProcessQualificationQuery request, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.TenantId == 0 ? 2 : _tenantContext.TenantId;
        var userId = (int)request.UserId;

        var allMatrices = await _configurationAdapter.GetAllMatrixConfigurations(brandId);
        if (allMatrices is null || allMatrices.Count == 0)
            return (false, new List<int>());

        allMatrices = allMatrices.OrderBy(m => m.MatrixType).ToList();

        var (commissions, totalWithdrawn, availableBalance) = await GetFinancialsAsync(userId, brandId);
        var qualifications = await EnsureQualificationsAsync(userId, allMatrices, commissions, totalWithdrawn, availableBalance, brandId);

        var userName = (await _accountServiceAdapter.GetUserInfo(userId, brandId))?.UserName ?? "Usuario";

        var anyQualified = false;
        var qualifiedMatrixTypes = new List<int>();
        var usersToVerify = new HashSet<int>();

        foreach (var m in allMatrices)
        {
            var q = qualifications.GetValueOrDefault(m.MatrixType);
            if (q == null) continue;

            var minCycle = qualifications.Count == 0 ? 0 : qualifications.Values.Min(x => x.QualificationCount);
            if (q.QualificationCount > minCycle) continue;

            if (!await CheckQualificationInternalAsync(userId, m.MatrixType, brandId)) continue;

            try
            {
                var newAvailable = await ApplyQualificationAsync(q, m, userName, availableBalance, brandId);
                await _accountServiceAdapter.PlaceUserInMatrix(
                    new MatrixRequest { UserId = userId, MatrixType = m.MatrixType }, brandId);
                var recipients = await ProcessMatrixCommissionsAsync(userId, m.MatrixType, q.QualificationCount, brandId);

                availableBalance = newAvailable;
                anyQualified = true;
                qualifiedMatrixTypes.Add(m.MatrixType);
                usersToVerify.UnionWith(recipients);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("balance"))
            {
                _logger.LogInformation(ex, "User {UserId} insufficient balance for matrix {MatrixType}", userId, m.MatrixType);
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing qualification for user {UserId} matrix {MatrixType}", userId, m.MatrixType);
            }
        }

        await SyncBalancesAsync(qualifications.Values, availableBalance);
        return (anyQualified, qualifiedMatrixTypes);
    }

    private async Task<(decimal commissions, decimal withdrawn, decimal available)> GetFinancialsAsync(int userId, long brandId)
    {
        var commissions = await _walletRepository.GetQualificationBalanceAsync(userId, brandId) ?? 0m;
        var totalWithdrawn = await _walletRequestRepository.GetTotalWithdrawnByAffiliateId(userId) ?? 0m;
        var availableBalance = await _walletRepository.GetAvailableBalanceByAffiliateId(userId, brandId);
        var pendingRequests = await _walletRequestRepository.GetTotalWalletRequestAmountByAffiliateId(userId, brandId);
        return (commissions, totalWithdrawn, availableBalance - pendingRequests);
    }

    private async Task<Dictionary<int, Domain.Models.MatrixQualification>> EnsureQualificationsAsync(
        int userId, List<MatrixConfiguration> allMatrices, decimal commissions, decimal totalWithdrawn,
        decimal availableBalance, long brandId)
    {
        var qualifications = (await _matrixQualificationRepository.GetAllByUserIdAsync(userId))
            .ToDictionary(q => q.MatrixType);
        var now = DateTime.Now;

        UserInfoResponse? userInfo = null;
        try { userInfo = await _accountServiceAdapter.GetUserInfo(userId, brandId); }
        catch (Exception ex) { _logger.LogWarning(ex, "Could not retrieve user info for {UserId}", userId); }
        var userExists = userInfo != null && userInfo.Id > 0;

        foreach (var matrixType in allMatrices.Select(m => m.MatrixType))
        {
            if (!qualifications.TryGetValue(matrixType, out var q))
            {
                if (!userExists) continue;
                q = new Domain.Models.MatrixQualification
                {
                    UserId = userId, MatrixType = matrixType, TotalEarnings = commissions,
                    WithdrawnAmount = totalWithdrawn, AvailableBalance = availableBalance,
                    CreatedAt = now, UpdatedAt = now
                };
                await _matrixQualificationRepository.CreateAsync(q);
                qualifications[matrixType] = q;
            }
            else
            {
                q.TotalEarnings = commissions;
                q.WithdrawnAmount = totalWithdrawn;
                q.AvailableBalance = availableBalance;
                q.UpdatedAt = now;
                await _matrixQualificationRepository.UpdateAsync(q);
            }
        }
        return qualifications;
    }

    private async Task<bool> CheckQualificationInternalAsync(int userId, int matrixType, long brandId)
    {
        var cfg = await _configurationAdapter.GetMatrixConfiguration(brandId, matrixType);
        if (cfg is null) return false;

        var qual = await _matrixQualificationRepository.GetByUserAndMatrixTypeAsync(userId, matrixType);
        if (qual == null) return false;

        var commissions = await _walletRepository.GetQualificationBalanceAsync(userId, brandId) ?? 0m;
        var withdrawn = await _walletRequestRepository.GetTotalWithdrawnByAffiliateId(userId) ?? 0m;
        qual.TotalEarnings = commissions;
        qual.WithdrawnAmount = withdrawn;
        qual.UpdatedAt = DateTime.Now;
        await _matrixQualificationRepository.UpdateAsync(qual);

        var cycle = qual.QualificationCount;
        var requiredAmount = cycle == 0 ? cfg.Threshold : cfg.RangeMax * cycle;
        return commissions >= requiredAmount;
    }

    private async Task<decimal> ApplyQualificationAsync(
        Domain.Models.MatrixQualification qualification, MatrixConfiguration matrixCfg,
        string userName, decimal availableBalance, long brandId)
    {
        if (availableBalance < matrixCfg.FeeAmount)
            throw new InvalidOperationException(
                $"Insufficient balance for qualification. Available: {availableBalance:C}, Required: {matrixCfg.FeeAmount:C}");

        await using var tx = await _matrixEarningsRepository.BeginTransactionAsync();
        try
        {
            var adminBase = Math.Round(matrixCfg.FeeAmount * 0.30m, 2);

            await _walletRepository.CreateAsync(new Domain.Models.Wallet
            {
                AffiliateId = qualification.UserId,
                UserId = 1,
                Concept = $"Automatic activation in {matrixCfg.MatrixName}",
                Detail = $"Activation cycle: {qualification.QualificationCount + 1}",
                Debit = matrixCfg.FeeAmount, Credit = 0,
                AffiliateUserName = userName,
                AdminUserName = Constants.RecycoinAdmin,
                Status = true,
                ConceptType = "purchasing_pool",
                BrandId = brandId, Date = DateTime.Now,
            });

            await _walletRepository.CreateAsync(new Domain.Models.Wallet
            {
                AffiliateId = 0, UserId = 1,
                Concept = $"Admin fee 30% - {matrixCfg.MatrixName} (User {qualification.UserId})",
                Detail = $"Cycle {qualification.QualificationCount + 1}",
                Debit = 0, Credit = adminBase,
                AffiliateUserName = Constants.RecycoinAdmin,
                AdminUserName = Constants.RecycoinAdmin,
                Status = true,
                ConceptType = nameof(WalletConceptType.commission_passed_wallet),
                BrandId = brandId, Date = DateTime.Now,
            });

            availableBalance -= matrixCfg.FeeAmount;
            qualification.IsQualified = true;
            qualification.QualificationCount += 1;
            qualification.AvailableBalance = availableBalance;
            qualification.LastQualificationTotalEarnings = qualification.TotalEarnings;
            qualification.LastQualificationWithdrawnAmount = qualification.WithdrawnAmount;
            qualification.LastQualificationDate = DateTime.Now;
            qualification.UpdatedAt = DateTime.Now;

            await _matrixQualificationRepository.UpdateAsync(qualification);
            await tx.CommitAsync();
            return availableBalance;
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    private async Task<HashSet<int>> ProcessMatrixCommissionsAsync(int userId, int matrixType, int qualificationCount, long brandId)
    {
        var usersReceivedCommissions = new HashSet<int>();
        await using var transaction = await _matrixEarningsRepository.BeginTransactionAsync();

        try
        {
            var matrixConfig = await _configurationAdapter.GetMatrixConfiguration(brandId, matrixType);
            if (matrixConfig is null)
                throw new InvalidOperationException("Error retrieving matrix config");

            var isActive = await _accountServiceAdapter.IsActiveInMatrix(
                new MatrixRequest { UserId = userId, MatrixType = matrixType }, brandId);
            if (!isActive)
                throw new UnauthorizedAccessException($"User {userId} does not have valid position in matrix {matrixType}");

            var commissionAmount = matrixConfig.FeeAmount * 0.1m;

            var allUplinePositions = await _accountServiceAdapter.GetUplinePositionsAsync(
                new MatrixRequest { UserId = userId, MatrixType = matrixType, Cycle = qualificationCount }, brandId);

            if (allUplinePositions != null)
            {
                var filteredPositions = allUplinePositions
                    .Where(p => p.MatrixType == matrixType)
                    .GroupBy(p => p.UserId)
                    .Select(g => g.OrderBy(p => p.Level).First())
                    .OrderBy(p => p.Level)
                    .Take(matrixConfig.Levels)
                    .ToList();

                int paidCount = 0;
                foreach (var uplineUserId in filteredPositions.Select(p => p.UserId))
                {
                    UserInfoResponse? uplineInfo = null;
                    try { uplineInfo = await _accountServiceAdapter.GetUserInfo((int)uplineUserId, brandId); }
                    catch { /* skip */ }
                    if (uplineInfo == null) continue;

                    var uplineQual = await _matrixQualificationRepository.GetByUserAndMatrixTypeAsync(uplineUserId, matrixType);
                    if (uplineQual is { IsQualified: true } && uplineQual.QualificationCount >= qualificationCount)
                    {
                        var earning = new MatrixEarning
                        {
                            UserId = uplineUserId, MatrixType = matrixType,
                            Amount = commissionAmount, SourceUserId = userId,
                            EarningType = "Matrix_Qualification", CreatedAt = DateTime.Now
                        };
                        await _matrixEarningsRepository.CreateAsync(earning, qualificationCount);
                        usersReceivedCommissions.Add((int)uplineUserId);
                        paidCount++;
                    }
                }

                var missedCount = Math.Max(0, filteredPositions.Count - paidCount);
                if (missedCount > 0)
                {
                    var adminMissed = Math.Round(missedCount * commissionAmount, 2);
                    await _walletRepository.CreateAsync(new Domain.Models.Wallet
                    {
                        AffiliateId = 0, UserId = 1,
                        Concept = $"Unpaid commissions x{missedCount} - {matrixConfig.MatrixName}",
                        Detail = $"User {userId} • Cycle {qualificationCount}",
                        Debit = 0, Credit = adminMissed,
                        AffiliateUserName = "adminrecycoin", AdminUserName = "adminrecycoin",
                        Status = true, ConceptType = "admin_missed_commission",
                        BrandId = brandId, Date = DateTime.Now,
                    });
                }
            }

            await transaction.CommitAsync();
            return usersReceivedCommissions;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task SyncBalancesAsync(IEnumerable<Domain.Models.MatrixQualification> qualifications, decimal finalAvailable)
    {
        var now = DateTime.Now;
        foreach (var qual in qualifications)
        {
            if (qual.AvailableBalance == finalAvailable) continue;
            qual.AvailableBalance = finalAvailable;
            qual.UpdatedAt = now;
            await _matrixQualificationRepository.UpdateAsync(qual);
        }
    }
}
