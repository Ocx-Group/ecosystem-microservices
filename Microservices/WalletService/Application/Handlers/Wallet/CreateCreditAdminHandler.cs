using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.Wallet;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class CreateCreditAdminHandler : IRequestHandler<CreateCreditAdminCommand, bool>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ICreditRepository _creditRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly ICacheService _cacheService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<CreateCreditAdminHandler> _logger;

    public CreateCreditAdminHandler(
        IWalletRepository walletRepository,
        ICreditRepository creditRepository,
        IAccountServiceAdapter accountServiceAdapter,
        ICacheService cacheService,
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext,
        ILogger<CreateCreditAdminHandler> logger)
    {
        _walletRepository = walletRepository;
        _creditRepository = creditRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _cacheService = cacheService;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(CreateCreditAdminCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var brandId = _tenantContext.TenantId;

        if (request.Amount == 0)
            return false;

        var user = await _accountServiceAdapter.GetUserInfo(request.AffiliateId, brandId);

        if (user is null)
            return false;

        var today = DateTime.Now;

        var adminUserName = brandId switch
        {
            1 => Constants.AdminEcosystemUserName,
            2 => Constants.RecycoinAdmin,
            3 => Constants.HouseCoinAdmin,
            4 => Constants.ExitoJuntosAdmin,
            _ => Constants.AdminEcosystemUserName
        };

        var wallet = new Domain.Models.Wallet
        {
            AffiliateId = user.Id,
            UserId = 1,
            Credit = (decimal)request.Amount,
            Debit = 0,
            Deferred = 0,
            Status = true,
            Concept = "Crédito administrativo",
            Date = today,
            Compression = false,
            AffiliateUserName = user.UserName,
            AdminUserName = adminUserName,
            ConceptType = WalletConceptType.balance_transfer.ToString(),
            BrandId = brandId,
        };

        var credit = new Credit
        {
            AffiliateId = user.Id,
            Concept = "Crédito administrativo",
            Credit1 = wallet.Credit,
            Debit = 0,
            Paid = 1,
            Request = 1,
            RequestDenied = 0,
            Iva = 0,
            Islr = 0
        };

        try
        {
            await _unitOfWork.BeginTransactionAsync();

            await _walletRepository.CreateAsync(wallet);
            await _creditRepository.CreateCredit(credit);

            await _unitOfWork.CommitAsync();
            await _cacheService.InvalidateBalanceAsync(user.Id);

            return true;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
