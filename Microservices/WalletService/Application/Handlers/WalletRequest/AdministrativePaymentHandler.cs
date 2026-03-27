using Ecosystem.Domain.Core.Caching;
using Ecosystem.WalletService.Application.Extensions;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.WalletRequest;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Extensions;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletRequest;

public class AdministrativePaymentHandler : IRequestHandler<AdministrativePaymentCommand, ResultResponse<int>>
{
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly ICacheService _cacheService;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<AdministrativePaymentHandler> _logger;

    public AdministrativePaymentHandler(
        IWalletRequestRepository walletRequestRepository,
        IAccountServiceAdapter accountServiceAdapter,
        ICacheService cacheService,
        ITenantContext tenantContext,
        ILogger<AdministrativePaymentHandler> logger)
    {
        _walletRequestRepository = walletRequestRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _cacheService = cacheService;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<ResultResponse<int>> Handle(AdministrativePaymentCommand command, CancellationToken cancellationToken)
    {
        var requestIds = command.RequestIds;
        var brandId = _tenantContext.TenantId;

        if (requestIds.Length == 0)
            return ResultResponse<int>.Fail("No se proporcionaron IDs de solicitudes");

        var requests = await _walletRequestRepository.GetWalletRequestsByIds(requestIds.ToList());

        if (!requests.Any())
            return ResultResponse<int>.Fail("No se encontraron solicitudes pendientes");

        var nonPendingRequests = requests.Where(r => r.Status != WalletRequestStatus.pending.ToByte()).ToList();
        if (nonPendingRequests.Any())
            return ResultResponse<int>.Fail(
                $"Existen {nonPendingRequests.Count} solicitudes que no están en estado pendiente");

        var userIds = requests.Select(x => x.AffiliateId).Distinct().ToList();
        var userInfoTasks = userIds.Select(id =>
            _accountServiceAdapter.GetUserInfo(id, brandId)).ToArray();
        var userInfoArray = await Task.WhenAll(userInfoTasks);
        var userInfoMap = userInfoArray
            .Where(u => u != null)
            .ToDictionary(u => u!.Id, u => u);

        var paymentRequests = requests
            .Where(r => userInfoMap.ContainsKey(r.AffiliateId))
            .Select(r => new WalletPaymentRequest
            {
                Id = r.Id,
                AffiliateId = r.AffiliateId,
                AffiliateUserName = userInfoMap[r.AffiliateId]!.UserName!,
                Amount = r.Amount
            })
            .ToList();

        if (!paymentRequests.Any())
            return ResultResponse<int>.Fail("No se pudo obtener información de los usuarios");

        var adminUserName = brandId switch
        {
            1 => Constants.AdminEcosystemUserName,
            2 => Constants.RecycoinAdmin,
            3 => Constants.HouseCoinAdmin,
            _ => Constants.AdminEcosystemUserName
        };

        var (success, processedCount, errorMessage) = await _walletRequestRepository
            .ProcessAdministrativePaymentAsync(paymentRequests, brandId, adminUserName);

        if (!success)
            return ResultResponse<int>.Fail($"Error al procesar pagos: {errorMessage}");

        var affiliateIds = paymentRequests.Select(p => p.AffiliateId).Distinct();
        await Task.WhenAll(affiliateIds.Select(id => _cacheService.InvalidateBalanceAsync(id)));

        return ResultResponse<int>.Ok(processedCount);
    }
}
