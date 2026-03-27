using AutoMapper;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.CoinPay;
using Ecosystem.WalletService.Domain.Requests.CoinPayRequest;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPay;

public class SendCoinPayFundsHandler : IRequestHandler<SendCoinPayFundsCommand, SendFundsResponse?>
{
    private readonly ICoinPayAdapter _coinPayAdapter;
    private readonly IMapper _mapper;
    private readonly ILogger<SendCoinPayFundsHandler> _logger;

    public SendCoinPayFundsHandler(
        ICoinPayAdapter coinPayAdapter,
        IMapper mapper,
        ILogger<SendCoinPayFundsHandler> logger)
    {
        _coinPayAdapter = coinPayAdapter;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SendFundsResponse?> Handle(SendCoinPayFundsCommand request, CancellationToken cancellationToken)
    {
        var sendRequest = _mapper.Map<SendFundRequest>(request);
        return await _coinPayAdapter.SendFunds(sendRequest);
    }
}
