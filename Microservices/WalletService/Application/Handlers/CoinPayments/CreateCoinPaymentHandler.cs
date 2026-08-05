using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.CoinPayments;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Extensions;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPayments;

public class CreateCoinPaymentHandler : IRequestHandler<CreateCoinPaymentCommand, CreateConPaymentsTransactionResponse?>
{
    private readonly ICoinPaymentsAdapter _adapter;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCoinPaymentHandler> _logger;

    public CreateCoinPaymentHandler(
        ICoinPaymentsAdapter adapter,
        ITransactionRepository transactionRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<CreateCoinPaymentHandler> logger)
    {
        _adapter = adapter;
        _transactionRepository = transactionRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateConPaymentsTransactionResponse?> Handle(
        CreateCoinPaymentCommand request, CancellationToken cancellationToken)
    {
        var paymentRequest = _mapper.Map<ConPaymentRequest>(request);

        ApplyDefaults(paymentRequest);

        var response = await _adapter.CreatePayment(paymentRequest);

        if (response?.Result?.Txn_Id is null)
        {
            _logger.LogWarning(
                "CoinPayments did not return a transaction id for affiliate {ItemNumber}: {Error}",
                paymentRequest.ItemNumber, response?.Error);

            return response;
        }

        await PersistTransaction(paymentRequest, response, cancellationToken);

        return response;
    }

    /// <summary>
    /// Normalises the request the way the old service did before quoting the provider: both
    /// currencies are forced to BEP20 regardless of what the client asked for, the product list
    /// travels in "custom" so the IPN can rebuild it, and a 1% surcharge is added on top.
    /// </summary>
    private static void ApplyDefaults(ConPaymentRequest request)
    {
        request.Currency1 = Constants.CoinPaymentsBnbCurrency;
        request.Currency2 = Constants.CoinPaymentsBnbCurrency;

        if (request.Products is { Count: > 0 })
            request.Custom = request.Products.ToJsonString();

        request.Amount += request.Amount * CoinPaymentsConstants.FeeRate;
    }

    private async Task PersistTransaction(
        ConPaymentRequest request,
        CreateConPaymentsTransactionResponse response,
        CancellationToken cancellationToken)
    {
        // item_number carries the affiliate id: it is the only identifier CoinPayments echoes
        // back on the IPN, so a transaction that cannot resolve it could never be credited.
        if (!int.TryParse(request.ItemNumber, out var affiliateId))
        {
            _logger.LogError(
                "CoinPayments transaction {TxnId} has a non-numeric item_number {ItemNumber}; it will not be credited",
                response.Result!.Txn_Id, request.ItemNumber);
            return;
        }

        var now = DateTime.UtcNow;

        await _transactionRepository.CreateTransaction(new Transaction
        {
            IdTransaction = response.Result!.Txn_Id!,
            AffiliateId = affiliateId,
            Amount = request.Amount,
            AmountReceived = Constants.EmptyValue,
            Products = request.Custom ?? string.Empty,
            Acredited = false,
            Status = CoinPaymentsConstants.WaitingStatusCode,
            PaymentMethod = CoinPaymentsConstants.TransactionPaymentMethod,
            Address = response.Result.Address,
            BrandId = _tenantContext.TenantId,
            CreatedAt = now,
            UpdatedAt = now
        });

        _logger.LogInformation(
            "CoinPayments transaction {TxnId} registered for affiliate {AffiliateId}",
            response.Result.Txn_Id, affiliateId);
    }
}
