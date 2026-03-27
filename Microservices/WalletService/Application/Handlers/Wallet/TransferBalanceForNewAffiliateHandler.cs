using AutoMapper;
using Ecosystem.Domain.Core.Caching;
using Ecosystem.WalletService.Application.Extensions;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.Wallet;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;
using Ecosystem.WalletService.Domain.Requests.WalletTransactionRequest;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class TransferBalanceForNewAffiliateHandler : IRequestHandler<TransferBalanceForNewAffiliateCommand, bool>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly IBonusRepository _bonusRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly IBalancePaymentStrategy _balancePaymentStrategy;
    private readonly ICacheService _cacheService;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<TransferBalanceForNewAffiliateHandler> _logger;

    public TransferBalanceForNewAffiliateHandler(
        IWalletRepository walletRepository,
        IWalletRequestRepository walletRequestRepository,
        IBonusRepository bonusRepository,
        IAccountServiceAdapter accountServiceAdapter,
        IBalancePaymentStrategy balancePaymentStrategy,
        ICacheService cacheService,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<TransferBalanceForNewAffiliateHandler> logger)
    {
        _walletRepository = walletRepository;
        _walletRequestRepository = walletRequestRepository;
        _bonusRepository = bonusRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _balancePaymentStrategy = balancePaymentStrategy;
        _cacheService = cacheService;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<bool> Handle(TransferBalanceForNewAffiliateCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var brandId = _tenantContext.TenantId;
        var today = DateTime.Now;
        var amount = 10;

        var userInfo = await _accountServiceAdapter.GetAffiliateByUserName(request.ToUserName, brandId);
        var currentUser = await _accountServiceAdapter.GetAffiliateByUserName(request.FromUserName, brandId);

        if (!userInfo.IsSuccessful)
            return false;

        if (string.IsNullOrEmpty(userInfo.Content))
            return false;

        var userResult = JsonConvert.DeserializeObject<UserAffiliateResponse>(currentUser.Content!);
        var result = JsonConvert.DeserializeObject<UserAffiliateResponse>(userInfo.Content!);

        var userBalance = await GetBalanceInformation(request.FromAffiliateId, brandId);

        if (userResult?.Data?.VerificationCode != request.SecurityCode)
            return false;

        if (amount > userBalance.AvailableBalance)
            return false;

        if (result?.Data?.Status != 1)
            return false;

        if (result.Data?.ActivationDate != null)
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
            AffiliateId = result.Data!.Id,
            AdminUserName = adminUserName,
            Status = true,
            UserId = Constants.AdminUserId,
            Credit = amount,
            Concept = $"{Constants.TransferToMembership} {request.FromUserName}",
            Support = null!,
            Date = today,
            Compression = false,
            AffiliateUserName = result.Data.UserName,
            ConceptType = WalletConceptType.balance_transfer
        };

        var debitWallet = _mapper.Map<Domain.Models.Wallet>(debitTransaction);
        var creditWallet = _mapper.Map<Domain.Models.Wallet>(creditTransaction);

        var success = await _walletRepository.CreateTransferBalance(debitWallet, creditWallet);

        if (!success)
            return false;

        await _cacheService.InvalidateBalanceAsync(debitTransaction.AffiliateId, creditTransaction.AffiliateId);

        var confirmPurchase = await PurchaseMembershipForNewAffiliates(creditTransaction);

        if (!confirmPurchase)
            return false;

        return confirmPurchase;
    }

    private async Task<Domain.DTOs.BalanceInformationDto.BalanceInformationDto> GetBalanceInformation(int affiliateId, long brandId)
    {
        var key = string.Format(CacheKeys.BalanceInformationModel2, affiliateId);
        var existsKey = await _cacheService.KeyExists(key);

        if (!existsKey)
        {
            var amountRequests = await _walletRequestRepository.GetTotalWalletRequestAmountByAffiliateId(affiliateId, brandId);
            var availableBalance = await _walletRepository.GetAvailableBalanceByAffiliateId(affiliateId, brandId);
            var reverseBalance = await _walletRepository.GetReverseBalanceByAffiliateId(affiliateId, brandId);
            var totalAcquisitions = await _walletRepository.GetTotalAcquisitionsByAffiliateId(affiliateId, brandId);
            var totalCommissionsPaid = await _walletRepository.GetTotalCommissionsPaid(affiliateId, brandId);
            var totalServiceBalance = await _walletRepository.GetTotalServiceBalance(affiliateId, brandId);
            var bonusAmount = await _bonusRepository.GetBonusAmountByAffiliateId(affiliateId);

            var response = new Domain.DTOs.BalanceInformationDto.BalanceInformationDto
            {
                AvailableBalance = availableBalance,
                ReverseBalance = reverseBalance ?? 0,
                TotalAcquisitions = Math.Round(totalAcquisitions ?? 0, 2),
                TotalCommissionsPaid = totalCommissionsPaid ?? 0,
                ServiceBalance = totalServiceBalance ?? 0,
                BonusAmount = bonusAmount,
            };

            if (amountRequests != 0m || response.ReverseBalance != 0m)
            {
                response.AvailableBalance -= amountRequests;
                response.AvailableBalance -= response.ReverseBalance;
            }

            await _cacheService.Set(key, response, TimeSpan.FromHours(1));
            return response;
        }

        return await _cacheService.Get<Domain.DTOs.BalanceInformationDto.BalanceInformationDto>(key)
               ?? new Domain.DTOs.BalanceInformationDto.BalanceInformationDto();
    }

    private async Task<bool> PurchaseMembershipForNewAffiliates(WalletTransactionRequest request)
    {
        var membership = new ProductsRequests
        {
            IdProduct = 1,
            Count = 1
        };

        var walletRequest = new WalletRequestModel
        {
            AffiliateId = request.AffiliateId,
            AffiliateUserName = request.AffiliateUserName!,
            PurchaseFor = Constants.EmptyValue,
            Bank = Constants.WalletBalance,
            PaymentMethod = 0,
            SecretKey = null,
            ReceiptNumber = null,
            ProductsList = new List<ProductsRequests> { membership }
        };

        await _cacheService.InvalidateBalanceAsync(request.AffiliateId);
        return await _balancePaymentStrategy.ExecuteMembershipPayment(walletRequest);
    }
}
