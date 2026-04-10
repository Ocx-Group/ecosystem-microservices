using AutoMapper;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.PaymentTransaction;
using Ecosystem.WalletService.Domain.DTOs.PaymentTransactionDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.PaymentTransaction;

public class CreatePaymentTransactionHandler : IRequestHandler<CreatePaymentTransactionCommand, PaymentTransactionDto?>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public CreatePaymentTransactionHandler(
        ITransactionRepository transactionRepository,
        IAccountServiceAdapter accountServiceAdapter,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<CreatePaymentTransactionHandler> logger)
    {
        _transactionRepository = transactionRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<PaymentTransactionDto?> Handle(CreatePaymentTransactionCommand request, CancellationToken cancellationToken)
    {
        var user = await _accountServiceAdapter.GetUserInfo(request.AffiliateId, _tenantContext.TenantId);
        if (user is null) return null;

        var transaction = _mapper.Map<Transaction>(request);
        transaction.Acredited = false;
        transaction.Status = 0;
        transaction.AmountReceived = 0;
        transaction.PaymentMethod = "wire_transfer";
        transaction.BrandId = _tenantContext.TenantId;

        var response = await _transactionRepository.CreateTransaction(transaction);
        if (response is null) return null;

        return _mapper.Map<PaymentTransactionDto>(response);
    }
}
