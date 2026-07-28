using AutoMapper;
using Ecosystem.Domain.Core.BrandConfiguration;
using Ecosystem.Domain.Core.Caching;
using Ecosystem.WalletService.Application.Extensions;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.WalletRequest;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.DTOs.WalletRequestDto;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Extensions;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Ecosystem.WalletService.Application.Handlers.WalletRequest;

public class CreateWalletRequestHandler : IRequestHandler<CreateWalletRequestCommand, ResultResponse<WalletRequestDto>>
{
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletPeriodRepository _walletPeriodRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<CreateWalletRequestHandler> _logger;
    private readonly IConfigurationAdapter _configurationAdapter;

    public CreateWalletRequestHandler(
        IWalletRequestRepository walletRequestRepository,
        IWalletRepository walletRepository,
        IWalletPeriodRepository walletPeriodRepository,
        IAccountServiceAdapter accountServiceAdapter,
        ICacheService cacheService,
        IMapper mapper,
        ITenantContext tenantContext,
        IConfigurationAdapter configurationAdapter,
        ILogger<CreateWalletRequestHandler> logger)
    {
        _walletRequestRepository = walletRequestRepository;
        _walletRepository = walletRepository;
        _walletPeriodRepository = walletPeriodRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _cacheService = cacheService;
        _mapper = mapper;
        _tenantContext = tenantContext;
        _configurationAdapter = configurationAdapter;
        _logger = logger;
    }

    public async Task<ResultResponse<WalletRequestDto>> Handle(CreateWalletRequestCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var brandId = _tenantContext.TenantId;
        var brandConfiguration = await _configurationAdapter.GetBrandConfiguration(
            brandId,
            cancellationToken);

        if (brandConfiguration is null)
            return ResultResponse<WalletRequestDto>.Fail("Configuración de marca no disponible");

        var isDateValid = await IsWithdrawalDateAllowed(brandConfiguration);
        if (!isDateValid)
            return ResultResponse<WalletRequestDto>.Fail("La fecha de retiro no está permitida");

        if (!await HasWalletAddress(request.AffiliateId, brandId))
            return ResultResponse<WalletRequestDto>.Fail("No se encontró dirección de wallet válida");

        var isValid = await _accountServiceAdapter.VerificationCode(request.VerificationCode, request.UserPassword, request.AffiliateId, brandId);
        if (!isValid)
            return ResultResponse<WalletRequestDto>.Fail("Código de verificación inválido");

        var available = await _walletRepository.GetAvailableBalanceByAffiliateId(request.AffiliateId, brandId);
        var reverse = await _walletRepository.GetReverseBalanceByAffiliateId(request.AffiliateId, brandId) ?? 0;
        available -= reverse;

        if (request.Amount > available)
            return ResultResponse<WalletRequestDto>.Fail("El monto excede el saldo disponible");

        if (brandConfiguration.Requires10PercentPurchaseRule &&
            !await CheckFor10PercentPurchaseEarnings(
                request.AffiliateId,
                brandId,
                brandConfiguration.DefaultPaymentGroupId))
            return ResultResponse<WalletRequestDto>.Fail("Necesita tener el 10% de lo que haya ganado en compras para retirar dinero");

        if (brandConfiguration.WithdrawalCapNoDirects is decimal configuredCap)
        {
            var cap = await CheckUserWithdrawalCap(
                request.AffiliateId,
                brandId,
                configuredCap);
            if (request.Amount > cap)
                return ResultResponse<WalletRequestDto>.Fail(
                    $"Sin directos el máximo que puede retirar es de ${cap}");
        }

        var walletRequest = new WalletsRequest
        {
            Amount = request.Amount,
            Concept = request.Concept!,
            Status = WalletRequestStatus.pending.ToByte(),
            AffiliateId = request.AffiliateId,
            AdminUserName = request.AffiliateName,
            CreationDate = DateTime.Now,
            OrderNumber = CommonExtensions.GenerateOrderNumber(request.AffiliateId),
            Type = nameof(WalletRequestType.withdrawal_request),
            InvoiceNumber = Constants.EmptyValue,
            BrandId = brandId
        };

        var created = await _walletRequestRepository.CreateWalletRequestAsync(walletRequest);

        await _cacheService.InvalidateBalanceAsync(request.AffiliateId);

        var dto = _mapper.Map<WalletRequestDto>(created);
        return ResultResponse<WalletRequestDto>.Ok(dto);
    }

    private async Task<bool> IsWithdrawalDateAllowed(
        BrandConfigurationDto configuration)
    {
        return configuration.WithdrawalValidationType switch
        {
            "None" => true,
            "DatabaseDriven" => await IsDatabaseDrivenWithdrawalDateAllowed(configuration),
            "FridayUtc" => IsFridayWithdrawalDateAllowed(configuration),
            _ => throw new InvalidOperationException(
                $"Unsupported withdrawal validation type '{configuration.WithdrawalValidationType}'.")
        };
    }

    private async Task<bool> IsDatabaseDrivenWithdrawalDateAllowed(
        BrandConfigurationDto configuration)
    {
        var timeZone = GetWithdrawalTimeZone(configuration);
        var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

        if (!IsInsideConfiguredHours(localDateTime.TimeOfDay, configuration))
            return false;

        var allowedDatesObjects = await _walletPeriodRepository.GetAllWalletsPeriods();
        var allowedDates = allowedDatesObjects.Select(wp => wp.Date).ToList();
        var localDateOnly = DateOnly.FromDateTime(localDateTime.Date);
        return allowedDates.Contains(localDateOnly);
    }

    private static bool IsFridayWithdrawalDateAllowed(
        BrandConfigurationDto configuration)
    {
        var timeZone = GetWithdrawalTimeZone(configuration);
        var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

        if (localDateTime.DayOfWeek != DayOfWeek.Friday)
            return false;

        return IsInsideConfiguredHours(localDateTime.TimeOfDay, configuration);
    }

    private static TimeZoneInfo GetWithdrawalTimeZone(
        BrandConfigurationDto configuration)
    {
        if (string.IsNullOrWhiteSpace(configuration.WithdrawalTimeZone))
            throw new InvalidOperationException(
                $"Withdrawal time zone is not configured for brand {configuration.BrandId}.");

        return TimeZoneInfo.FindSystemTimeZoneById(configuration.WithdrawalTimeZone);
    }

    private static bool IsInsideConfiguredHours(
        TimeSpan currentTime,
        BrandConfigurationDto configuration)
    {
        if (configuration.WithdrawalStartHour is not int startHour ||
            configuration.WithdrawalEndHour is not int endHour)
        {
            throw new InvalidOperationException(
                $"Withdrawal hours are not configured for brand {configuration.BrandId}.");
        }

        var startTime = TimeSpan.FromHours(startHour);
        var endTime = TimeSpan.FromHours(endHour);
        return currentTime >= startTime && currentTime <= endTime;
    }

    private async Task<bool> HasWalletAddress(int affiliateId, long brandId)
    {
        var btcAddresses = await _accountServiceAdapter.GetAffiliateBtcByAffiliateId(affiliateId, brandId);
        return btcAddresses is not null && btcAddresses.Any();
    }

    private async Task<bool> CheckFor10PercentPurchaseEarnings(
        int affiliateId,
        long brandId,
        int? paymentGroupId)
    {
        var commissions = await _walletRepository.GetTotalCommissionsPaid(affiliateId, brandId);
        var purchases = await _walletRepository.GetTotalAcquisitionsByAffiliateId(
            affiliateId,
            brandId,
            paymentGroupId);

        var minimumPurchaseRequired = commissions * 0.10m;

        return purchases >= minimumPurchaseRequired;
    }

    private async Task<decimal> CheckUserWithdrawalCap(
        int affiliateId,
        long brandId,
        decimal configuredCap)
    {
        var users = new[] { affiliateId };

        var directUsersArray = await _accountServiceAdapter.GetHave2Children(users, brandId);
        var directUsers = directUsersArray?.ToList() ?? new List<int>();

        if (directUsers.Contains(affiliateId))
            return decimal.MaxValue;

        return configuredCap;
    }
}
