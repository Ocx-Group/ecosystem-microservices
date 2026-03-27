using AutoMapper;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.Pagadito;
using Ecosystem.WalletService.Domain.CustomModels;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;
using Ecosystem.WalletService.Domain.Requests.PagaditoRequest;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ecosystem.WalletService.Application.Handlers.Pagadito;

public class CreatePagaditoTransactionHandler : IRequestHandler<CreatePagaditoTransactionCommand, string?>
{
    private readonly IPagaditoAdapter _pagaditoAdapter;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreatePagaditoTransactionHandler> _logger;

    public CreatePagaditoTransactionHandler(
        IPagaditoAdapter pagaditoAdapter,
        ITransactionRepository transactionRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<CreatePagaditoTransactionHandler> logger)
    {
        _pagaditoAdapter = pagaditoAdapter;
        _transactionRepository = transactionRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string?> Handle(CreatePagaditoTransactionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating Pagadito transaction for affiliate {AffiliateId}", request.AffiliateId);

        var connectResponse = await _pagaditoAdapter.ConnectAsync();
        if (connectResponse == null || string.IsNullOrEmpty(connectResponse.Value))
            return "This connect is not valid";

        var pagaditoRequest = _mapper.Map<CreatePagaditoTransactionRequest>(request);
        var pagaditoTransaction = _mapper.Map<CreatePagaditoTransaction>(pagaditoRequest);
        pagaditoTransaction.Token = connectResponse.Value;
        pagaditoTransaction.Ern = Guid.NewGuid().ToString();

        var executeTransaction = await _pagaditoAdapter.ExecuteTransaction(pagaditoTransaction);
        if (string.IsNullOrEmpty(executeTransaction?.Value))
            return "This transaction is not valid";

        var productDetails = request.Details.Select(detail => new ProductRequest
        {
            ProductId = int.Parse(detail.UrlProduct!),
            Quantity = detail.Quantity,
        }).ToList();

        var today = DateTime.Now;
        var paymentTransaction = new Transaction
        {
            IdTransaction = pagaditoTransaction.Ern,
            AffiliateId = request.AffiliateId,
            Amount = pagaditoTransaction.Amount,
            AmountReceived = 0,
            Products = JsonSerializer.Serialize(productDetails),
            PaymentMethod = "pagadito",
            Status = 0,
            Acredited = false,
            CreatedAt = today,
            UpdatedAt = today,
            BrandId = _tenantContext.TenantId
        };

        await _transactionRepository.CreateTransaction(paymentTransaction);

        return executeTransaction.Value;
    }
}
