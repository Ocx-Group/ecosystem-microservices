using AutoMapper;
using Ecosystem.Domain.Core.Caching;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.Wallet;
using Ecosystem.WalletService.Application.Queries.Wallet;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;
using Ecosystem.WalletService.Domain.Requests.WalletTransactionRequest;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.WalletService.Application.Extensions;
using MediatR;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class TransferBalanceForNewAffiliateHandler : IRequestHandler<TransferBalanceForNewAffiliateCommand, bool>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly IMediator _mediator;
    private readonly ICacheService _cacheService;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public TransferBalanceForNewAffiliateHandler(
        IWalletRepository walletRepository,
        IAccountServiceAdapter accountServiceAdapter,
        IBalancePaymentStrategy balancePaymentStrategy,
        IMediator mediator,
        ICacheService cacheService,
        ITenantContext tenantContext,
        IMapper mapper)
    {
        _walletRepository = walletRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _mediator = mediator;
        _cacheService = cacheService;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<bool> Handle(TransferBalanceForNewAffiliateCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var brandId = _tenantContext.TenantId;
        var today = DateTime.Now;
        var amount = 10;

        var userInfo = await _accountServiceAdapter.GetAffiliateByUserName(request.ToUserName, brandId);
        var currentUser = await _accountServiceAdapter.GetAffiliateByUserName(request.FromUserName, brandId);

        if (userInfo is null)
            return false;

        if (currentUser is null)
            return false;

        var userBalance = await _mediator.Send(new GetBalanceInformationQuery(request.FromAffiliateId), cancellationToken);

        if (currentUser.VerificationCode != request.SecurityCode)
            return false;

        if (amount > userBalance.AvailableBalance)
            return false;

        if (userInfo.Status != 1)
            return false;

        if (userInfo.ActivationDate != null)
            return false;

        var adminUserName = brandId switch
        {
            1 => Constants.AdminEcosystemUserName,
            2 => Constants.RecycoinAdmin,
            3 => Constants.HouseCoinAdmin,
            4 => Constants.ExitoJuntosAdmin,
            _ => Constants.AdminEcosystemUserName
        };

        var debitTransaction = new WalletTransactionRequest
        {
            Debit = amount,
            Deferred = Constants.EmptyValue,
            Detail = null,
            AffiliateId = request.FromAffiliateId,
            AdminUserName = adminUserName,
            Status = true,
            UserId = Constants.AdminUserId,
            Credit = Constants.EmptyValue,
            Concept = $"{Constants.TransferForMembership} {request.ToUserName}",
            Support = null!,
            Date = today,
            Compression = false,
            AffiliateUserName = request.FromUserName,
            ConceptType = WalletConceptType.balance_transfer
        };

        var creditTransaction = new WalletTransactionRequest
        {
            Debit = Constants.EmptyValue,
            Deferred = Constants.EmptyValue,
            Detail = null,
            AffiliateId = userInfo.Id,
            AdminUserName = adminUserName,
            Status = true,
            UserId = Constants.AdminUserId,
            Credit = amount,
            Concept = $"{Constants.TransferToMembership} {request.FromUserName}",
            Support = null!,
            Date = today,
            Compression = false,
            AffiliateUserName = userInfo.UserName,
            ConceptType = WalletConceptType.balance_transfer
        };

        var debitWallet = _mapper.Map<Domain.Models.Wallet>(debitTransaction);
        var creditWallet = _mapper.Map<Domain.Models.Wallet>(creditTransaction);

        var success = await _walletRepository.CreateTransferBalance(debitWallet, creditWallet);

        if (!success)
            return false;

        await _cacheService.InvalidateBalanceAsync(debitTransaction.AffiliateId, creditTransaction.AffiliateId);

        var membershipRequest = new WalletRequestModel
        {
            AffiliateId = creditTransaction.AffiliateId,
            AffiliateUserName = creditTransaction.AffiliateUserName!,
            PurchaseFor = Constants.EmptyValue,
            Bank = Constants.WalletBalance,
            PaymentMethod = 0,
            SecretKey = null,
            ReceiptNumber = null,
            ProductsList = new List<ProductsRequests>
            {
                new() { IdProduct = 1, Count = 1 }
            }
        };

        return await _mediator.Send(new PayMembershipWithBalanceCommand(membershipRequest), cancellationToken);
    }
}
