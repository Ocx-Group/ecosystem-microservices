using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.WalletModel1B;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletModel1B;

public class CreateServiceBalanceAdmin1BHandler : IRequestHandler<CreateServiceBalanceAdmin1BCommand, bool>
{
    private readonly IWalletModel1BRepository _walletModel1BRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly ICacheService _cacheService;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<CreateServiceBalanceAdmin1BHandler> _logger;

    public CreateServiceBalanceAdmin1BHandler(
        IWalletModel1BRepository walletModel1BRepository,
        IAccountServiceAdapter accountServiceAdapter,
        ICacheService cacheService,
        ITenantContext tenantContext,
        ILogger<CreateServiceBalanceAdmin1BHandler> logger)
    {
        _walletModel1BRepository = walletModel1BRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _cacheService = cacheService;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(CreateServiceBalanceAdmin1BCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var brandId = _tenantContext.TenantId;

        if (request.Amount == 0)
            return false;

        var user = await _accountServiceAdapter.GetUserInfo(request.AffiliateId, brandId);

        if (user is null)
            return false;

        var credit = new CreditTransactionRequest
        {
            AdminUserName = Constants.AdminEcosystemUserName,
            AffiliateId = user.Id,
            Concept = Constants.AdminCredit,
            Credit = request.Amount,
            AffiliateUserName = user.UserName,
            ConceptType = Constants.AdminCredit,
            UserId = Constants.AdminUserId
        };

        var result = await _walletModel1BRepository.CreditServiceBalanceTransaction(credit);
        if (!result)
            return false;

        await _cacheService.InvalidateBalanceAsync(user.Id);
        return true;
    }
}
