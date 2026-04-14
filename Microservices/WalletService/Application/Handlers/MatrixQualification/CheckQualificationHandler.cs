using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.MatrixQualification;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.MatrixQualification;

public class CheckQualificationHandler : IRequestHandler<CheckQualificationCommand, bool>
{
    private readonly IMatrixQualificationRepository _matrixQualificationRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly IConfigurationAdapter _configurationAdapter;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<CheckQualificationHandler> _logger;

    public CheckQualificationHandler(
        IMatrixQualificationRepository matrixQualificationRepository,
        IWalletRepository walletRepository,
        IWalletRequestRepository walletRequestRepository,
        IAccountServiceAdapter accountServiceAdapter,
        IConfigurationAdapter configurationAdapter,
        ITenantContext tenantContext,
        ILogger<CheckQualificationHandler> logger)
    {
        _matrixQualificationRepository = matrixQualificationRepository;
        _walletRepository = walletRepository;
        _walletRequestRepository = walletRequestRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _configurationAdapter = configurationAdapter;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(CheckQualificationCommand request, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.TenantId == 0 ? 2 : _tenantContext.TenantId;

        var cfg = await _configurationAdapter.GetMatrixConfiguration(brandId, request.MatrixType);
        if (cfg is null)
            throw new InvalidDataException("Error retrieving matrix configuration");

        var qual = await _matrixQualificationRepository.GetByUserAndMatrixTypeAsync(request.UserId, request.MatrixType);
        if (qual == null)
        {
            UserInfoResponse? userInfo = null;
            try { userInfo = await _accountServiceAdapter.GetUserInfo(request.UserId, brandId); }
            catch (Exception ex) { _logger.LogWarning(ex, "Could not retrieve user info for {UserId}", request.UserId); }
            if (userInfo == null) return false;

            qual = new Domain.Models.MatrixQualification
            {
                UserId = request.UserId, MatrixType = request.MatrixType,
                CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now
            };
            await _matrixQualificationRepository.CreateAsync(qual);
            return false;
        }

        var commissions = await _walletRepository.GetQualificationBalanceAsync(request.UserId, brandId) ?? 0m;
        var withdrawn = await _walletRequestRepository.GetTotalWithdrawnByAffiliateId(request.UserId) ?? 0m;
        qual.TotalEarnings = commissions;
        qual.WithdrawnAmount = withdrawn;
        qual.UpdatedAt = DateTime.Now;
        await _matrixQualificationRepository.UpdateAsync(qual);

        var cycle = qual.QualificationCount;
        var requiredAmount = cycle == 0 ? cfg!.Threshold : cfg!.RangeMax * cycle;
        return commissions >= requiredAmount;
    }
}
