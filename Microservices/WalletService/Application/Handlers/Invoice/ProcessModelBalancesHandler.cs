using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.Invoice;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.DTOs.InvoiceDto;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Invoice;

public class ProcessModelBalancesHandler : IRequestHandler<ProcessModelBalancesCommand, ModelBalancesAndInvoicesDto?>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IWalletModel1ARepository _walletModel1ARepository;
    private readonly IWalletModel1BRepository _walletModel1BRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ICacheService _cacheService;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<ProcessModelBalancesHandler> _logger;

    public ProcessModelBalancesHandler(
        IInvoiceRepository invoiceRepository,
        IWalletModel1ARepository walletModel1ARepository,
        IWalletModel1BRepository walletModel1BRepository,
        IWalletRepository walletRepository,
        ICacheService cacheService,
        ITenantContext tenantContext,
        ILogger<ProcessModelBalancesHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _walletModel1ARepository = walletModel1ARepository;
        _walletModel1BRepository = walletModel1BRepository;
        _walletRepository = walletRepository;
        _cacheService = cacheService;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<ModelBalancesAndInvoicesDto?> Handle(ProcessModelBalancesCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var brandId = _tenantContext.TenantId;
        var totalModels = request.Model1AAmount + request.Model1BAmount + request.Model2Amount;

        if (request.InvoiceId.Length == 0)
            return null;

        var (validInvoiceIds, affiliateId, totalInvoices) = await ProcessInvoices(request.InvoiceId, brandId);

        if (affiliateId == 0 || totalModels > totalInvoices)
            return null;

        bool isAnyCreditTransactionSuccessful = false;

        if (request.Model1AAmount > 0)
            isAnyCreditTransactionSuccessful |=
                await CreditAmountToWallet(affiliateId, request.UserName, request.Model1AAmount, "Model1A");

        if (request.Model1BAmount > 0)
            isAnyCreditTransactionSuccessful |=
                await CreditAmountToWallet(affiliateId, request.UserName, request.Model1BAmount, "Model1B");

        if (request.Model2Amount > 0)
            isAnyCreditTransactionSuccessful |=
                await CreditAmountToWallet(affiliateId, request.UserName, request.Model2Amount, "Model2");

        if (isAnyCreditTransactionSuccessful && validInvoiceIds.Any())
        {
            await _invoiceRepository.DeleteMultipleInvoicesAndDetailsAsync(validInvoiceIds.ToArray(), brandId);
        }

        return new ModelBalancesAndInvoicesDto
        {
            UserName = request.UserName,
            Model1AAmount = request.Model1AAmount,
            Model1BAmount = request.Model1BAmount,
            Model2Amount = request.Model2Amount,
            InvoiceId = validInvoiceIds.ToArray()
        };
    }

    private async Task<(List<long>, int, decimal)> ProcessInvoices(long[] invoiceIds, long brandId)
    {
        var totalInvoices = 0m;
        var validInvoiceIds = new HashSet<long>();
        int affiliateId = 0;

        foreach (var invoiceId in invoiceIds)
        {
            if (!validInvoiceIds.Contains(invoiceId))
            {
                var invoice = await _invoiceRepository.GetInvoiceById(invoiceId, brandId);

                if (invoice != null)
                {
                    if (affiliateId == 0)
                        affiliateId = invoice.AffiliateId;
                    else if (affiliateId != invoice.AffiliateId)
                    {
                        return (new List<long>(), 0, 0m);
                    }

                    totalInvoices += invoice.TotalInvoice ?? 0;
                    validInvoiceIds.Add(invoiceId);
                }
            }
        }

        return (validInvoiceIds.ToList(), affiliateId, totalInvoices);
    }

    private async Task<bool> CreditAmountToWallet(int affiliateId, string userName, decimal amount, string model)
    {
        if (amount <= 0) return false;

        var creditTransactionRequest = new CreditTransactionRequest
        {
            AffiliateId = affiliateId,
            UserId = Constants.AdminUserId,
            Concept = Constants.BalanceRefunds,
            Credit = Convert.ToDouble(amount),
            AffiliateUserName = userName,
            AdminUserName = Constants.AdminEcosystemUserName,
            ConceptType = WalletConceptType.revert_pool.ToString()
        };

        try
        {
            switch (model)
            {
                case "Model1A":
                    await RemoveCacheKey(affiliateId, CacheKeys.BalanceInformationModel1A);
                    await RemoveCacheKey(affiliateId, CacheKeys.BalanceInformationModel1B);
                    await RemoveCacheKey(affiliateId, CacheKeys.BalanceInformationModel2);
                    return await _walletModel1ARepository.CreditTransaction(creditTransactionRequest);
                case "Model1B":
                    await RemoveCacheKey(affiliateId, CacheKeys.BalanceInformationModel1A);
                    await RemoveCacheKey(affiliateId, CacheKeys.BalanceInformationModel1B);
                    await RemoveCacheKey(affiliateId, CacheKeys.BalanceInformationModel2);
                    return await _walletModel1BRepository.CreditTransaction(creditTransactionRequest);
                case "Model2":
                    await RemoveCacheKey(affiliateId, CacheKeys.BalanceInformationModel1A);
                    await RemoveCacheKey(affiliateId, CacheKeys.BalanceInformationModel1B);
                    await RemoveCacheKey(affiliateId, CacheKeys.BalanceInformationModel2);
                    return await _walletRepository.CreditTransaction(creditTransactionRequest);
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    private async Task RemoveCacheKey(int affiliateId, string stringKey)
    {
        var key = string.Format(stringKey, affiliateId);
        var existsKey = await _cacheService.KeyExists(key);

        if (existsKey)
            await _cacheService.Delete(key);
    }
}
