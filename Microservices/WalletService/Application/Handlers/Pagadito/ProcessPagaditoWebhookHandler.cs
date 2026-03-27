using Ecosystem.WalletService.Application.Commands.Pagadito;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Requests.PagaditoRequest;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Ecosystem.WalletService.Application.Handlers.Pagadito;

public class ProcessPagaditoWebhookHandler : IRequestHandler<ProcessPagaditoWebhookCommand, bool>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<ProcessPagaditoWebhookHandler> _logger;

    public ProcessPagaditoWebhookHandler(
        ITransactionRepository transactionRepository,
        ITenantContext tenantContext,
        ILogger<ProcessPagaditoWebhookHandler> logger)
    {
        _transactionRepository = transactionRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(ProcessPagaditoWebhookCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing Pagadito webhook");

        var webhookRequest = JsonConvert.DeserializeObject<WebHookRequest>(request.RequestBody);
        if (webhookRequest?.Resource == null)
            return false;

        var brandId = _tenantContext.TenantId;
        var existingTransaction = await _transactionRepository.GetTransactionByIdTransaction(
            webhookRequest.Resource.Ern!, brandId);

        if (existingTransaction is null)
        {
            _logger.LogWarning("Transaction not found for ERN {Ern}", webhookRequest.Resource.Ern);
            return false;
        }

        if (existingTransaction.Acredited)
            return false;

        if (webhookRequest.Resource.Status == "REGISTERED")
        {
            existingTransaction.Status = 0;
            existingTransaction.Acredited = false;
        }
        else if (webhookRequest.Resource.Status == "EXPIRED")
        {
            existingTransaction.Status = -1;
            existingTransaction.Acredited = false;
        }
        else if (webhookRequest.Resource.Status == "COMPLETED" && !existingTransaction.Acredited)
        {
            existingTransaction.Status = 100;
            if (decimal.TryParse(webhookRequest.Resource.Amount?.Total,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out var amountReceived))
            {
                existingTransaction.AmountReceived = amountReceived;
            }
            existingTransaction.Acredited = true;
            existingTransaction.Reference = webhookRequest.Resource.Reference;
        }

        existingTransaction.UpdatedAt = DateTime.UtcNow;
        await _transactionRepository.UpdateTransactionAsync(existingTransaction);
        _logger.LogInformation("Pagadito webhook processed for transaction {Ern}", webhookRequest.Resource.Ern);

        return true;
    }
}
