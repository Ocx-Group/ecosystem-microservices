using AutoMapper;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.WalletRequest;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.DTOs.WalletRequestDto;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Extensions;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletRequest;

public class CreateWalletRequestRevertHandler : IRequestHandler<CreateWalletRequestRevertCommand, WalletRequestDto?>
{
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IInvoiceDetailRepository _invoiceDetailRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IBalancePaymentStrategy _balancePaymentStrategy;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly IMapper _mapper;
    private readonly ITenantContext _tenantContext;

    public CreateWalletRequestRevertHandler(
        IWalletRequestRepository walletRequestRepository,
        IInvoiceRepository invoiceRepository,
        IInvoiceDetailRepository invoiceDetailRepository,
        IWalletRepository walletRepository,
        IBalancePaymentStrategy balancePaymentStrategy,
        IAccountServiceAdapter accountServiceAdapter,
        IMapper mapper,
        ITenantContext tenantContext,
        ILogger<CreateWalletRequestRevertHandler> logger)
    {
        _walletRequestRepository = walletRequestRepository;
        _invoiceRepository = invoiceRepository;
        _invoiceDetailRepository = invoiceDetailRepository;
        _walletRepository = walletRepository;
        _balancePaymentStrategy = balancePaymentStrategy;
        _accountServiceAdapter = accountServiceAdapter;
        _mapper = mapper;
        _tenantContext = tenantContext;
    }

    public async Task<WalletRequestDto?> Handle(CreateWalletRequestRevertCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var brandId = _tenantContext.TenantId;

        var response = await _invoiceRepository.GetInvoiceById(request.InvoiceId, brandId);
        var userInfoResponse = await _accountServiceAdapter.GetUserInfo(response!.AffiliateId, brandId);

        var amountReverted = response.TotalInvoice * (decimal?)0.90;
        var leftOverBalance = response.TotalInvoice - amountReverted;

        var creditRequest = new CreditTransactionRequest
        {
            AffiliateId = request.AffiliateId,
            UserId = Constants.AdminUserId,
            Concept = Constants.RevertEcoPoolConcept + $" Factura# {request.InvoiceId}",
            Credit = Convert.ToDouble(amountReverted),
            AffiliateUserName = userInfoResponse!.UserName,
            AdminUserName = Constants.AdminEcosystemUserName,
            ConceptType = nameof(WalletConceptType.revert_pool),
            BrandId = brandId
        };

        var walletRequest = new WalletsRequest
        {
            AffiliateId = response.AffiliateId,
            AdminUserName = userInfoResponse.UserName,
            OrderNumber = CommonExtensions.GenerateOrderNumber(request.AffiliateId),
            InvoiceNumber = response.Id,
            Amount = response.TotalInvoice ?? 0,
            Concept = request.Concept,
            Type = nameof(WalletRequestType.revert_invoice_request),
            Status = WalletRequestStatus.pending.ToByte(),
            CreationDate = DateTime.Now,
            AttentionDate = null,
            BrandId = brandId
        };

        walletRequest = await _walletRequestRepository.CreateWalletRequestAsync(walletRequest);

        var walletMovement = await _walletRequestRepository.GetWalletRequestsByInvoiceNumber(request.InvoiceId);

        if (walletMovement == null)
            return null;

        if (walletRequest != null)
        {
            await DeleteInvoiceAndDetails(request.InvoiceId, brandId);
            await _walletRepository.CreditTransaction(creditRequest);
            await CreateCustomEcoPool(userInfoResponse, leftOverBalance, brandId);
            walletMovement.Status = WalletRequestStatus.approved.ToByte();
            walletMovement.AttentionDate = DateTime.Now;
            await _walletRequestRepository.UpdateWalletRequestsAsync(walletMovement);
        }

        return _mapper.Map<WalletRequestDto>(walletRequest);
    }

    private async Task<bool> DeleteInvoiceAndDetails(int invoiceNumber, long brandId)
    {
        try
        {
            var invoiceResponse = await _invoiceRepository.GetInvoiceById(invoiceNumber, brandId);
            var invoiceDetailResponse = await _invoiceDetailRepository.GetInvoiceDetailByInvoiceIdAsync(invoiceNumber);

            if (invoiceResponse is null || !invoiceDetailResponse.Any())
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

    private async Task<bool> CreateCustomEcoPool(UserInfoResponse? user, decimal? leftOverBalance, long brandId)
    {
        if (user is null)
            return false;

        var walletRequest = new Domain.Requests.WalletRequest.WalletRequest
        {
            AffiliateId = user.Id,
            AffiliateUserName = user.UserName!,
            PurchaseFor = 0,
            Bank = Constants.WalletBalance,
            PaymentMethod = 0,
            SecretKey = null,
            ReceiptNumber = leftOverBalance.ToString(),
            BrandId = brandId,
            ProductsList = new List<ProductsRequests>
            {
                new ProductsRequests
                {
                    Count = 1,
                    IdProduct = 23
                }
            },
        };

        return await _balancePaymentStrategy.ExecuteAdminPayment(walletRequest, leftOverBalance);
    }
}
