using System.Collections.Concurrent;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.MatrixQualification;
using Ecosystem.WalletService.Application.Queries.MatrixQualification;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.MatrixQualification;

public class ProcessAllQualificationsHandler : IRequestHandler<ProcessAllQualificationsCommand, BatchProcessingResult>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly IConfigurationAdapter _configurationAdapter;
    private readonly IMediator _mediator;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<ProcessAllQualificationsHandler> _logger;

    public ProcessAllQualificationsHandler(
        IWalletRepository walletRepository,
        IAccountServiceAdapter accountServiceAdapter,
        IConfigurationAdapter configurationAdapter,
        IMediator mediator,
        ITenantContext tenantContext,
        ILogger<ProcessAllQualificationsHandler> logger)
    {
        _walletRepository = walletRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _configurationAdapter = configurationAdapter;
        _mediator = mediator;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<BatchProcessingResult> Handle(ProcessAllQualificationsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting matrix qualifications processing. IDs: {Count}", request.UserIds?.Length ?? 0);
        var brandId = _tenantContext.TenantId == 0 ? 2 : _tenantContext.TenantId;

        var result = new BatchProcessingResult
        {
            StartTime = DateTime.Now,
            ProcessedUsers = new ConcurrentBag<UserProcessingResult>()
        };

        var usersToProcess = request.UserIds?.Length > 0
            ? request.UserIds
            : (await _walletRepository.GetUserIdsWithCommissionsGreaterThanOrEqualTo50(brandId))
                .Where(u => u != 0).Select(u => (int)u).ToArray();

        const int maxDop = 8;
        using var throttler = new SemaphoreSlim(maxDop);

        var tasks = usersToProcess.Select(async uid =>
        {
            await throttler.WaitAsync(cancellationToken);
            try
            {
                var userInfo = await _accountServiceAdapter.GetUserInfo(uid, brandId);
                if (userInfo == null)
                {
                    _logger.LogWarning("Skipping non-existing user {UserId}", uid);
                    return;
                }

                var (qualified, matrixTypes) = await _mediator.Send(
                    new ProcessQualificationQuery(uid), cancellationToken);

                var userResult = new UserProcessingResult
                {
                    UserId = uid,
                    ProcessedTime = DateTime.Now,
                    WasQualified = qualified,
                    MatricesQualified = matrixTypes.Select(mt => new MatrixQualificationInfo
                    {
                        MatrixType = mt,
                        QualificationDate = DateTime.Now
                    }).ToList()
                };
                result.ProcessedUsers.Add(userResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing user {UserId}", uid);
                result.ProcessedUsers.Add(new UserProcessingResult
                {
                    UserId = uid, ProcessedTime = DateTime.Now,
                    WasQualified = false, ErrorMessage = ex.Message
                });
            }
            finally
            {
                throttler.Release();
            }
        });

        await Task.WhenAll(tasks);

        result.TotalProcessed = result.ProcessedUsers.Count;
        result.TotalQualified = result.ProcessedUsers.Count(u => u.WasQualified);
        result.EndTime = DateTime.Now;
        result.ElapsedTimeSeconds = (result.EndTime - result.StartTime).TotalSeconds;
        return result;
    }
}
