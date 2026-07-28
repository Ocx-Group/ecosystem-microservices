using AutoMapper;
using Ecosystem.Domain.Core.Caching;
using Ecosystem.WalletService.Application.Extensions;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.Wallet;
using Ecosystem.WalletService.Application.Queries.Wallet;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Extensions;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Requests.TransferBalanceRequest;
using Ecosystem.WalletService.Domain.Requests.WalletTransactionRequest;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class TransferBalanceHandler : IRequestHandler<TransferBalanceCommand, ServicesResponse>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly IMediator _mediator;
    private readonly ICacheService _cacheService;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly IConfigurationAdapter _configurationAdapter;

    public TransferBalanceHandler(
        IWalletRepository walletRepository,
        IAccountServiceAdapter accountServiceAdapter,
        IMediator mediator,
        ICacheService cacheService,
        ITenantContext tenantContext,
        IMapper mapper,
        IConfigurationAdapter configurationAdapter,
        ILogger<TransferBalanceHandler> logger)
    {
        _walletRepository = walletRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _mediator = mediator;
        _cacheService = cacheService;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _configurationAdapter = configurationAdapter;
    }

    public async Task<ServicesResponse> Handle(TransferBalanceCommand command, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.TenantId;
        var brandConfiguration = await _configurationAdapter.GetBrandConfiguration(
            brandId,
            cancellationToken);

        if (brandConfiguration is null)
            return new ServicesResponse
                { Success = false, Message = "Configuración de marca no disponible", Code = 503 };

        var data = CommonExtensions.DecryptObject<TransferBalanceRequest>(command.Encrypted);

        var today = DateTime.Now;
        var amount = data.Amount;
        var currentUser = await _accountServiceAdapter.GetAffiliateByUserName(data.FromUserName, brandId);
        var userInfo = await _accountServiceAdapter.GetAffiliateByUserName(data.ToUserName, brandId);
        var isActivePool = await _walletRepository.IsActivePoolGreaterThanOrEqualTo25(data.FromAffiliateId, brandId);

        if (!isActivePool && brandConfiguration.PoolValidationRequired)
            return new ServicesResponse { Success = false, Message = "No tiene un Pool activo", Code = 400 };

        if (userInfo is null)
            return new ServicesResponse { Success = false, Message = "Error", Code = 400 };

        if (currentUser is null)
            return new ServicesResponse { Success = false, Message = "Error", Code = 400 };

        var userBalance = await _mediator.Send(new GetBalanceInformationQuery(data.FromAffiliateId), cancellationToken);

        if (currentUser.VerificationCode != data.SecurityCode)
            return new ServicesResponse
                { Success = false, Message = "El código de seguridad no coincidec.", Code = 400 };

        if (amount > userBalance.AvailableBalance)
            return new ServicesResponse
                { Success = false, Message = "El monto es mayor al saldo disponible.", Code = 400 };

        if (userInfo.Status != 1)
            return new ServicesResponse
                { Success = false, Message = "El estatus del afiliado a transferir es inactivo.", Code = 400 };

        var adminUserName = brandConfiguration.AdminUserName;

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
            AffiliateId = userInfo.Id,
            AdminUserName = adminUserName,
            Status = true,
            UserId = 1,
            Credit = amount,
            Concept = "Transferencia de saldo del afiliado " + data.FromUserName,
            Support = null!,
            Date = today,
            Compression = false,
            AffiliateUserName = userInfo.UserName,
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
}
