using Ecosystem.WalletService.Application.Commands.Invoice;
using Ecosystem.WalletService.Domain.CustomModels;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Invoice;

public class HandleDebitTransactionHandler : IRequestHandler<HandleDebitTransactionCommand, InvoicesSpResponse?>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ILogger<HandleDebitTransactionHandler> _logger;

    public HandleDebitTransactionHandler(
        IInvoiceRepository invoiceRepository,
        ILogger<HandleDebitTransactionHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _logger = logger;
    }

    public async Task<InvoicesSpResponse?> Handle(HandleDebitTransactionCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _invoiceRepository.HandleDebitTransaction(command.Request);

            if (result == null)
            {
                _logger.LogWarning("No se pudo procesar la transacción de débito para el afiliado: {AffiliateId}",
                    command.Request.AffiliateId);
                return null;
            }

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error al procesar la transacción de débito para el afiliado: {AffiliateId}",
                command.Request.AffiliateId);
            throw;
        }
    }
}
