using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.Wallet;
using Ecosystem.WalletService.Domain.DTOs.WalletDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class GetBalanceInformationAdminHandler : IRequestHandler<GetBalanceInformationAdminQuery, BalanceInformationAdminDto>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GetBalanceInformationAdminHandler> _logger;
    private readonly IConfigurationAdapter _configurationAdapter;

    public GetBalanceInformationAdminHandler(
        IWalletRepository walletRepository,
        IWalletRequestRepository walletRequestRepository,
        IInvoiceRepository invoiceRepository,
        IAccountServiceAdapter accountServiceAdapter,
        ITenantContext tenantContext,
        IConfigurationAdapter configurationAdapter,
        ILogger<GetBalanceInformationAdminHandler> logger)
    {
        _walletRepository = walletRepository;
        _walletRequestRepository = walletRequestRepository;
        _invoiceRepository = invoiceRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _tenantContext = tenantContext;
        _configurationAdapter = configurationAdapter;
        _logger = logger;
    }

    public async Task<BalanceInformationAdminDto> Handle(GetBalanceInformationAdminQuery request, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.TenantId;
        var brandConfiguration = await _configurationAdapter.GetBrandConfiguration(
            brandId,
            cancellationToken)
            ?? throw new InvalidOperationException(
                $"Active brand configuration not found for brand {brandId}.");

        var totalActiveMembers = await _accountServiceAdapter.GetTotalActiveMembers(brandId);

        var paymentGroupId = brandConfiguration.DefaultPaymentGroupId
            ?? throw new InvalidOperationException(
                $"Default payment group is not configured for brand {brandId}.");

        var enabledAffiliates = totalActiveMembers;
        var walletProfit = await _walletRepository.GetAvailableBalanceAdmin(brandId);
        var amountRequests = await _walletRequestRepository.GetTotalWalletRequestAmount(brandId);
        var reverseBalance = await _walletRepository.GetTotalReverseBalance(brandId);
        var paidCommissions = await _walletRepository.GetTotalCommissionsPaid(brandId);
        var calculatedCommissions = await _invoiceRepository.GetTotalAdquisitionsAdmin(brandId, paymentGroupId);
        var totalCommissionsEarned = await _walletRepository.GetCommissionsForAdminAsync(brandId);

        var information = new BalanceInformationAdminDto
        {
            EnabledAffiliates = enabledAffiliates,
            WalletProfit = walletProfit,
            CommissionsPaid = paidCommissions,
            CalculatedCommissions = calculatedCommissions,
            TotalReverseBalance = reverseBalance,
            TotalCommissionsEarned = totalCommissionsEarned
        };

        if (amountRequests == 0m && reverseBalance == 0) return information;

        information.WalletProfit -= amountRequests;
        information.WalletProfit -= reverseBalance;

        return information;
    }
}
