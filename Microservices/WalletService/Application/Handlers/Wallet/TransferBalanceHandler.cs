using AutoMapper;
using Ecosystem.Domain.Core.Caching;
using Ecosystem.WalletService.Application.Extensions;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.Wallet;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.DTOs.BalanceInformationDto;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Extensions;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Requests.TransferBalanceRequest;
using Ecosystem.WalletService.Domain.Requests.WalletTransactionRequest;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class TransferBalanceHandler : IRequestHandler<TransferBalanceCommand, ServicesResponse>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly IBonusRepository _bonusRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly ICacheService _cacheService;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<TransferBalanceHandler> _logger;

    public TransferBalanceHandler(
        IWalletRepository walletRepository,
        IWalletRequestRepository walletRequestRepository,
        IBonusRepository bonusRepository,
        IAccountServiceAdapter accountServiceAdapter,
        ICacheService cacheService,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<TransferBalanceHandler> logger)
    {
        _walletRepository = walletRepository;
        _walletRequestRepository = walletRequestRepository;
        _bonusRepository = bonusRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _cacheService = cacheService;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ServicesResponse> Handle(TransferBalanceCommand command, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.TenantId;
        var data = CommonExtensions.DecryptObject<TransferBalanceRequest>(command.Encrypted);

        var today = DateTime.Now;
        var amount = data.Amount;
        var currentUser = await _accountServiceAdapter.GetAffiliateByUserName(data.FromUserName, brandId);
        var userInfo = await _accountServiceAdapter.GetAffiliateByUserName(data.ToUserName, brandId);
        var isActivePool = await _walletRepository.IsActivePoolGreaterThanOrEqualTo25(data.FromAffiliateId, brandId);

        if (!isActivePool && brandId == 1)
            return new ServicesResponse { Success = false, Message = "No tiene un Pool activo", Code = 400 };

        if (!userInfo.IsSuccessful)
            return new ServicesResponse { Success = false, Message = "Error", Code = 400 };

        if (string.IsNullOrEmpty(userInfo.Content))
            return new ServicesResponse { Success = false, Message = "Error", Code = 400 };

        var currentUserResult = JsonConvert.DeserializeObject<UserAffiliateResponse>(currentUser.Content!);
        var result = JsonConvert.DeserializeObject<UserAffiliateResponse>(userInfo.Content!);
        var userBalance = await GetBalanceInformation(data.FromAffiliateId, brandId);

        if (currentUserResult?.Data?.VerificationCode != data.SecurityCode)
            return new ServicesResponse
                { Success = false, Message = "El código de seguridad no coincidec.", Code = 400 };

        if (amount > userBalance.AvailableBalance)
            return new ServicesResponse
                { Success = false, Message = "El monto es mayor al saldo disponible.", Code = 400 };

        if (result?.Data?.Status != 1)
            return new ServicesResponse
                { Success = false, Message = "El estatus del afiliado a transferir es inactivo.", Code = 400 };

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
            Deferred = 0,
            Detail = null,
            AffiliateId = data.FromAffiliateId,
            AdminUserName = adminUserName,
            Status = true,
            UserId = 1,
            Credit = 0,
            Concept = "Transferencia de saldo al afiliado " + data.ToUserName,
            Support = null!,
            Date = today,
            Compression = false,
            AffiliateUserName = data.FromUserName,
            ConceptType = WalletConceptType.balance_transfer,
            BrandId = brandId,
        };

        var creditTransaction = new WalletTransactionRequest
        {
            Debit = 0,
            Deferred = 0,
            Detail = null,
            AffiliateId = result!.Data!.Id,
            AdminUserName = adminUserName,
            Status = true,
            UserId = 1,
            Credit = amount,
            Concept = "Transferencia de saldo del afiliado " + data.FromUserName,
            Support = null!,
            Date = today,
            Compression = false,
            AffiliateUserName = result.Data.UserName,
            ConceptType = WalletConceptType.balance_transfer,
            BrandId = brandId,
        };

        var debitWallet = _mapper.Map<Domain.Models.Wallet>(debitTransaction);
        var creditWallet = _mapper.Map<Domain.Models.Wallet>(creditTransaction);

        var success = await _walletRepository.CreateTransferBalance(debitWallet, creditWallet);

        if (!success)
            return new ServicesResponse { Success = false, Message = "No se pudo crear la transferencia.", Code = 400 };

        await _cacheService.InvalidateBalanceAsync(debitTransaction.AffiliateId, creditTransaction.AffiliateId);
        return new ServicesResponse
            { Success = true, Message = "La transferencia se ha creado correctamente.", Code = 200 };
    }

    private async Task<BalanceInformationDto> GetBalanceInformation(int affiliateId, long brandId)
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

            var response = new BalanceInformationDto
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

        return await _cacheService.Get<BalanceInformationDto>(key) ?? new BalanceInformationDto();
    }
}
