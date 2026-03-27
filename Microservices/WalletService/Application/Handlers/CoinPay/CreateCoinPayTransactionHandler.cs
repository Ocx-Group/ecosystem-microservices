using AutoMapper;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.CoinPay;
using Ecosystem.WalletService.Domain.Requests.CoinPayRequest;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPay;

public class CreateCoinPayTransactionHandler : IRequestHandler<CreateCoinPayTransactionCommand, CreateTransactionResponse?>
{
    private readonly ICoinPayAdapter _coinPayAdapter;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCoinPayTransactionHandler> _logger;

    public CreateCoinPayTransactionHandler(
        ICoinPayAdapter coinPayAdapter,
        IMapper mapper,
        ILogger<CreateCoinPayTransactionHandler> logger)
    {
        _coinPayAdapter = coinPayAdapter;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateTransactionResponse?> Handle(CreateCoinPayTransactionCommand request, CancellationToken cancellationToken)
    {
        var createRequest = _mapper.Map<CreateTransactionRequest>(request);
        return await _coinPayAdapter.CreateTransaction(createRequest);
    }
}
