using AutoMapper;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.CoinPayments;
using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPayments;

public class CreateCoinPaymentHandler : IRequestHandler<CreateCoinPaymentCommand, CreateConPaymentsTransactionResponse?>
{
    private readonly ICoinPaymentsAdapter _adapter;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCoinPaymentHandler> _logger;

    public CreateCoinPaymentHandler(
        ICoinPaymentsAdapter adapter,
        IMapper mapper,
        ILogger<CreateCoinPaymentHandler> logger)
    {
        _adapter = adapter;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateConPaymentsTransactionResponse?> Handle(CreateCoinPaymentCommand request, CancellationToken cancellationToken)
    {
        var paymentRequest = _mapper.Map<ConPaymentRequest>(request);
        return await _adapter.CreatePayment(paymentRequest);
    }
}
