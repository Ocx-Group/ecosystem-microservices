using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.PaymentTransaction;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.PaymentTransaction;

public class ConfirmPaymentHandler : IRequestHandler<ConfirmPaymentCommand, bool>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<ConfirmPaymentHandler> _logger;

    public ConfirmPaymentHandler(
        ITransactionRepository transactionRepository,
        ITenantContext tenantContext,
        ILogger<ConfirmPaymentHandler> logger)
    {
        _transactionRepository = transactionRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.TenantId;
        var paymentTransaction = await _transactionRepository.GetPaymentTransactionById(request.Id, brandId);
        if (paymentTransaction is null) return false;

        paymentTransaction.Acredited = true;
        paymentTransaction.Status = 100;
        paymentTransaction.AmountReceived = paymentTransaction.Amount;
        paymentTransaction.UpdatedAt = DateTime.UtcNow;

        var result = await _transactionRepository.UpdateTransactionAsync(paymentTransaction);
        return result is not null;
    }
}
