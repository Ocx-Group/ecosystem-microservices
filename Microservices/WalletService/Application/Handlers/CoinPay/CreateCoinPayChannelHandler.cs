using AutoMapper;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.CoinPay;
using Ecosystem.WalletService.Domain.Requests.CoinPayRequest;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPay;

public class CreateCoinPayChannelHandler : IRequestHandler<CreateCoinPayChannelCommand, CreateChannelResponse?>
{
    private readonly ICoinPayAdapter _coinPayAdapter;
    private readonly IMapper _mapper;

    public CreateCoinPayChannelHandler(
        ICoinPayAdapter coinPayAdapter,
        IMapper mapper,
        ILogger<CreateCoinPayChannelHandler> logger)
    {
        _coinPayAdapter = coinPayAdapter;
        _mapper = mapper;
    }

    public async Task<CreateChannelResponse?> Handle(CreateCoinPayChannelCommand request, CancellationToken cancellationToken)
    {
        var channelRequest = _mapper.Map<CreateChannelRequest>(request);
        return await _coinPayAdapter.CreateChannel(channelRequest);
    }
}
