using System.Text.Json;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.CoinPay;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Requests.CoinPayRequest;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPay;

public class CreateCoinPayTransactionHandler
    : IRequestHandler<CreateCoinPayTransactionCommand, CreateTransactionResponse?>
{
    private readonly ICoinPayAdapter _coinPayAdapter;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<CreateCoinPayTransactionHandler> _logger;

    public CreateCoinPayTransactionHandler(
        ICoinPayAdapter coinPayAdapter,
        ITransactionRepository transactionRepository,
        ITenantContext tenantContext,
        ILogger<CreateCoinPayTransactionHandler> logger)
    {
        _coinPayAdapter = coinPayAdapter;
        _transactionRepository = transactionRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<CreateTransactionResponse?> Handle(
        CreateCoinPayTransactionCommand request, CancellationToken cancellationToken)
    {
        var products = JsonSerializer.Serialize(request.Products);

        var paymentRequest = new PaymentRequest
        {
            Amount = request.Amount,
            IdCurrency = Constants.UsdtIdCurrency,
            Details = products
        };

        var result = await _coinPayAdapter.CreateTransaction(paymentRequest);

        if (result?.Data is null)
        {
            _logger.LogWarning(
                "CoinPay did not return a payment request for affiliate {AffiliateId}", request.AffiliateId);
            return result;
        }

        var now = DateTime.UtcNow;
        var idTransaction = result.Data.IdTransaction.ToString();

        var transaction = new Transaction
        {
            IdTransaction = idTransaction,
            AffiliateId = request.AffiliateId,
            Amount = result.Data.Amount,
            AmountReceived = Constants.EmptyValue,
            Products = result.Data.Details ?? products,
            Acredited = false,
            Status = result.StatusCode,
            PaymentMethod = Constants.CoinPay,
            CreatedAt = now,
            UpdatedAt = now,
            BrandId = _tenantContext.TenantId
        };

        await _transactionRepository.CreateTransaction(transaction);

        _logger.LogInformation(
            "CoinPay payment request {IdTransaction} recorded for affiliate {AffiliateId}",
            idTransaction, request.AffiliateId);

        return result;
    }
}
