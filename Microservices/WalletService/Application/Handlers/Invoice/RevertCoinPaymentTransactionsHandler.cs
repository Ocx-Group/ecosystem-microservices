using System.Text;
using Ecosystem.WalletService.Application.Commands.Invoice;
using Ecosystem.WalletService.Domain.CustomModels;
using Ecosystem.WalletService.Domain.Extensions;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Invoice;

public class RevertCoinPaymentTransactionsHandler : IRequestHandler<RevertCoinPaymentTransactionsCommand, bool>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<RevertCoinPaymentTransactionsHandler> _logger;

    public RevertCoinPaymentTransactionsHandler(
        IInvoiceRepository invoiceRepository,
        ITransactionRepository transactionRepository,
        ITenantContext tenantContext,
        ILogger<RevertCoinPaymentTransactionsHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _transactionRepository = transactionRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(RevertCoinPaymentTransactionsCommand command, CancellationToken cancellationToken)
    {
        var builder = new StringBuilder();
        var brandId = _tenantContext.TenantId;

        builder.AppendLine("[InvoiceHandler] | RevertUnconfirmedOrUnpaidTransactions | Started");
        var transactionsResults = await _transactionRepository.GetAllUnconfirmedOrUnpaidTransactions(brandId);
        var idsTransactions = transactionsResults.Select(e => e.IdTransaction).ToList();

        if (idsTransactions is { Count: 0 })
        {
            _logger.LogWarning(builder.ToString());
            return false;
        }

        builder.AppendLine($"[InvoiceHandler] | RevertUnconfirmedOrUnpaidTransactions | idsTransactions {idsTransactions.ToJsonString()}");
        var invoicesResult = await _invoiceRepository.GetInvoicesByReceiptNumber(idsTransactions);
        var invoicesToRevert = invoicesResult.Select(e => new InvoiceNumber { InvoiceNumberValue = e.Id }).ToList();

        if (invoicesToRevert is { Count: 0 })
        {
            _logger.LogWarning(builder.ToString());
            return false;
        }

        builder.AppendLine($"[InvoiceHandler] | RevertUnconfirmedOrUnpaidTransactions | invoicesToRevert {idsTransactions.ToJsonString()}");
        await _invoiceRepository.RevertCoinPaymentTransactions(invoicesToRevert);

        builder.AppendLine("[InvoiceHandler] | RevertUnconfirmedOrUnpaidTransactions | Completed");
        _logger.LogInformation(builder.ToString());

        return true;
    }
}
