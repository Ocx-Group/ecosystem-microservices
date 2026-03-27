using AutoMapper;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.CoinPay;
using Ecosystem.WalletService.Domain.Requests.CoinPayRequest;
using Ecosystem.WalletService.Domain.Responses.BaseResponses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPay;

public class CreateCoinPayAddressHandler : IRequestHandler<CreateCoinPayAddressCommand, CreateAddressResponse?>
{
    private readonly ICoinPayAdapter _coinPayAdapter;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCoinPayAddressHandler> _logger;

    public CreateCoinPayAddressHandler(
        ICoinPayAdapter coinPayAdapter,
        IMapper mapper,
        ILogger<CreateCoinPayAddressHandler> logger)
    {
        _coinPayAdapter = coinPayAdapter;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateAddressResponse?> Handle(CreateCoinPayAddressCommand request, CancellationToken cancellationToken)
    {
        var addressRequest = _mapper.Map<CreateAddresRequest>(request);
        return await _coinPayAdapter.CreateAddress(addressRequest);
    }
}
