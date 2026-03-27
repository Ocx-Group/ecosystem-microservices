using Ecosystem.Domain.Core.Caching;
using Ecosystem.WalletService.Application.Extensions;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.WalletModel1A;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletModel1A;

public class CreateServiceBalanceAdmin1AHandler : IRequestHandler<CreateServiceBalanceAdmin1ACommand, bool>
{
    private readonly IWalletModel1ARepository _walletModel1ARepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly ICacheService _cacheService;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<CreateServiceBalanceAdmin1AHandler> _logger;

    public CreateServiceBalanceAdmin1AHandler(
        IWalletModel1ARepository walletModel1ARepository,
        IAccountServiceAdapter accountServiceAdapter,
        ICacheService cacheService,
        ITenantContext tenantContext,
        ILogger<CreateServiceBalanceAdmin1AHandler> logger)
    {
        _walletModel1ARepository = walletModel1ARepository;
        _accountServiceAdapter = accountServiceAdapter;
        _cacheService = cacheService;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(CreateServiceBalanceAdmin1ACommand command, CancellationToken cancellationToken)
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

        var result = await _walletModel1ARepository.CreditServiceBalanceTransaction(credit);
        if (!result)
            return false;

        await _cacheService.InvalidateBalanceAsync(user.Id);
        return true;
    }
}
