using Ecosystem.WalletService.Application.Commands.CoinPayments;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPayments;

public class ProcessCoinPaymentsIpnHandler : IRequestHandler<ProcessCoinPaymentsIpnCommand, bool>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<ProcessCoinPaymentsIpnHandler> _logger;

    public ProcessCoinPaymentsIpnHandler(
        ITransactionRepository transactionRepository,
        ITenantContext tenantContext,
        ILogger<ProcessCoinPaymentsIpnHandler> logger)
    {
        _transactionRepository = transactionRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(ProcessCoinPaymentsIpnCommand request, CancellationToken cancellationToken)
    {
        var ipn = request.Request;
        _logger.LogInformation("Processing CoinPayments IPN for txn_id {TxnId}, status {Status}", ipn.txn_id, ipn.status);

        var transaction = await _transactionRepository.GetTransactionByTxnId(ipn.txn_id);
        if (transaction is null)
        {
            _logger.LogWarning("Transaction not found for txn_id {TxnId}", ipn.txn_id);
            return false;
        }

        if (transaction.Acredited)
            return true;

        transaction.Status = ipn.status;
        transaction.AmountReceived = ipn.received_amount;
        transaction.UpdatedAt = DateTime.UtcNow;

        if (ipn.status >= 100)
        {
            transaction.Acredited = true;
        }
        else if (ipn.status < 0)
        {
            transaction.Acredited = false;
        }

        await _transactionRepository.UpdateTransactionAsync(transaction);
        _logger.LogInformation("CoinPayments IPN processed for txn_id {TxnId}", ipn.txn_id);

        return true;
    }
}
