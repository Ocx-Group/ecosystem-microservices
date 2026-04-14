using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.MatrixQualification;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Requests.MatrixRequest;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.MatrixQualification;

public class ProcessQualificationAdminHandler : IRequestHandler<ProcessQualificationAdminCommand, bool>
{
    private readonly IMatrixQualificationRepository _matrixQualificationRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly IConfigurationAdapter _configurationAdapter;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<ProcessQualificationAdminHandler> _logger;

    public ProcessQualificationAdminHandler(
        IMatrixQualificationRepository matrixQualificationRepository,
        IAccountServiceAdapter accountServiceAdapter,
        IConfigurationAdapter configurationAdapter,
        ITenantContext tenantContext,
        ILogger<ProcessQualificationAdminHandler> logger)
    {
        _matrixQualificationRepository = matrixQualificationRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _configurationAdapter = configurationAdapter;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(ProcessQualificationAdminCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var brandId = _tenantContext.TenantId;
            var matrixConfig = await _configurationAdapter.GetMatrixConfiguration(brandId, request.MatrixType);

            if (matrixConfig is null)
                throw new InvalidOperationException("Error retrieving matrix configuration");

            var isActive = await _accountServiceAdapter.IsActiveInMatrix(
                new MatrixRequest { UserId = request.UserId, MatrixType = request.MatrixType }, brandId);
            if (isActive) return false;

            var qualification = await _matrixQualificationRepository.GetByUserAndMatrixTypeAsync(request.UserId, request.MatrixType);
            if (qualification is { IsQualified: true }) return false;

            if (qualification == null)
            {
                qualification = new Domain.Models.MatrixQualification
                {
                    UserId = request.UserId, MatrixType = request.MatrixType,
                    TotalEarnings = matrixConfig.Threshold, WithdrawnAmount = 0,
                    AvailableBalance = 0, IsQualified = true, QualificationCount = 1,
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
                qualification.UpdatedAt = DateTime.Now;
                await _matrixQualificationRepository.UpdateAsync(qualification);
            }

            await _accountServiceAdapter.PlaceUserInMatrix(
                new MatrixRequest { UserId = request.UserId, MatrixType = request.MatrixType }, brandId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error in admin matrix placement: {Message}", ex.Message);
            return false;
        }
    }
}
