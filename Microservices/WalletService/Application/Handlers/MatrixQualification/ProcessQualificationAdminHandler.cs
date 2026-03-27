using System.Net;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.MatrixQualification;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Requests.MatrixRequest;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Ecosystem.WalletService.Application.Handlers.MatrixQualification;

public class ProcessQualificationAdminHandler : IRequestHandler<ProcessQualificationAdminCommand, bool>
{
    private readonly IMatrixQualificationRepository _matrixQualificationRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly IConfigurationAdapter _configurationAdapter;
    private readonly IMediator _mediator;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<ProcessQualificationAdminHandler> _logger;

    public ProcessQualificationAdminHandler(
        IMatrixQualificationRepository matrixQualificationRepository,
        IAccountServiceAdapter accountServiceAdapter,
        IConfigurationAdapter configurationAdapter,
        IMediator mediator,
        ITenantContext tenantContext,
        ILogger<ProcessQualificationAdminHandler> logger)
    {
        _matrixQualificationRepository = matrixQualificationRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _configurationAdapter = configurationAdapter;
        _mediator = mediator;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(ProcessQualificationAdminCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var brandId = _tenantContext.TenantId;
            var matrixConfigResponse = await _configurationAdapter.GetMatrixConfiguration(brandId, request.MatrixType);
            var matrixConfig = JsonConvert.DeserializeObject<MatrixConfigurationResponse>(matrixConfigResponse.Content!);

            if (matrixConfigResponse.Content == null || matrixConfigResponse.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException($"Error retrieving matrix configuration: {matrixConfigResponse.StatusCode}");
            if (matrixConfig?.Data is null)
                throw new InvalidDataException("Matrix configuration data is missing");

            var positionResponse = await _accountServiceAdapter.IsActiveInMatrix(
                new MatrixRequest { UserId = request.UserId, MatrixType = request.MatrixType }, brandId);
            var existing = JsonConvert.DeserializeObject<MatrixPositionResponse>(positionResponse.Content!);
            if (positionResponse.IsSuccessful && existing?.Data == true) return false;

            var qualification = await _matrixQualificationRepository.GetByUserAndMatrixTypeAsync(request.UserId, request.MatrixType);
            if (qualification is { IsQualified: true }) return false;

            if (qualification == null)
            {
                qualification = new Domain.Models.MatrixQualification
                {
                    UserId = request.UserId, MatrixType = request.MatrixType,
                    TotalEarnings = matrixConfig.Data.Threshold, WithdrawnAmount = 0,
                    AvailableBalance = 0, IsQualified = true, QualificationCount = 1,
                    LastQualificationTotalEarnings = matrixConfig.Data.Threshold,
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
