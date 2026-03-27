using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.Wallet;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Extensions;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class HandleRevertTransactionHandler : IRequestHandler<HandleRevertTransactionCommand, bool>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IInvoiceDetailRepository _invoiceDetailRepository;
    private readonly ICacheService _cacheService;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<HandleRevertTransactionHandler> _logger;

    public HandleRevertTransactionHandler(
        IWalletRepository walletRepository,
        IWalletRequestRepository walletRequestRepository,
        IInvoiceRepository invoiceRepository,
        IInvoiceDetailRepository invoiceDetailRepository,
        ICacheService cacheService,
        ITenantContext tenantContext,
        ILogger<HandleRevertTransactionHandler> logger)
    {
        _walletRepository = walletRepository;
        _walletRequestRepository = walletRequestRepository;
        _invoiceRepository = invoiceRepository;
        _invoiceDetailRepository = invoiceDetailRepository;
        _cacheService = cacheService;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(HandleRevertTransactionCommand command, CancellationToken cancellationToken)
    {
        var walletRequest = await _walletRequestRepository.GetWalletRequestsByInvoiceNumber(command.Id);

        if (walletRequest == null)
            return false;

        var creditRequest = CreateCreditTransactionRequest(walletRequest);

        switch (command.Option)
        {
            case 0:
                walletRequest.Status = WalletRequestStatus.cancel.ToByte();
                await UpdateWalletRequestAsync(walletRequest);
                break;

            case 1:
                if (!await DeleteInvoiceAndDetails(walletRequest.InvoiceNumber))
                    return false;

                if (!await _walletRepository.CreditTransaction(creditRequest))
                    return false;

                walletRequest.Status = WalletRequestStatus.approved.ToByte();
                await UpdateWalletRequestAsync(walletRequest);
                break;

            default:
                walletRequest.Status = WalletRequestStatus.cancel.ToByte();
                break;
        }

        await _cacheService.InvalidateBalanceAsync(walletRequest.AffiliateId);
        return true;
    }

    private CreditTransactionRequest CreateCreditTransactionRequest(WalletsRequest walletRequest)
    {
        return new CreditTransactionRequest
        {
            AffiliateId = walletRequest.AffiliateId,
            UserId = Constants.AdminUserId,
            Concept = Constants.RevertEcoPoolConcept + $" Factura# {walletRequest.InvoiceNumber}",
            Credit = Convert.ToDouble(walletRequest.Amount),
            AffiliateUserName = walletRequest.AdminUserName!,
            AdminUserName = Constants.AdminEcosystemUserName,
            ConceptType = WalletConceptType.revert_pool.ToString()
        };
    }

    private async Task<bool> DeleteInvoiceAndDetails(long invoiceNumber)
    {
        try
        {
            var brandId = _tenantContext.TenantId;
            var invoiceResponse = await _invoiceRepository.GetInvoiceById(invoiceNumber, brandId);
            var invoiceDetailResponse = await _invoiceDetailRepository.GetInvoiceDetailByInvoiceIdAsync(invoiceNumber);

            if (invoiceResponse is null || invoiceDetailResponse.Any() == false)
                return false;

            await _invoiceRepository.DeleteInvoiceAsync(invoiceResponse);
            await _invoiceDetailRepository.DeleteBulkInvoiceDetailAsync(invoiceDetailResponse);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private async Task UpdateWalletRequestAsync(WalletsRequest walletRequest)
    {
        walletRequest.AttentionDate = DateTime.Now;
        await _walletRequestRepository.UpdateWalletRequestsAsync(walletRequest);
    }
}
