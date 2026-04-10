using Ecosystem.WalletService.Application.Commands.CoinPay;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPay;

public class ProcessCoinPayWebhookHandler : IRequestHandler<ProcessCoinPayWebhookCommand, bool>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<ProcessCoinPayWebhookHandler> _logger;

    public ProcessCoinPayWebhookHandler(
        ITransactionRepository transactionRepository,
        ITenantContext tenantContext,
        ILogger<ProcessCoinPayWebhookHandler> logger)
    {
        _transactionRepository = transactionRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(ProcessCoinPayWebhookCommand request, CancellationToken cancellationToken)
    {
        var notification = request.Request;
        _logger.LogInformation("Processing CoinPay webhook for transaction {TransactionId}", notification.IdTransaction);

        var transaction = await _transactionRepository.GetTransactionByReference(notification.IdExternalReference);
        if (transaction is null)
        {
            _logger.LogWarning("Transaction not found for reference {Reference}", notification.IdExternalReference);
            return false;
        }

        if (transaction.Acredited)
            return true;

        transaction.Status = notification.TransactionStatus.Id;
        transaction.AmountReceived = notification.Amount;
        transaction.UpdatedAt = DateTime.UtcNow;

        await _transactionRepository.UpdateTransactionAsync(transaction);
        _logger.LogInformation("CoinPay webhook processed for transaction {TransactionId}", notification.IdTransaction);

        return true;
    }
}
