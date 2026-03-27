using AutoMapper;
using Ecosystem.WalletService.Application.Queries.WalletPeriod;
using Ecosystem.WalletService.Domain.DTOs.WalletPeriodDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletPeriod;

public class GetWalletPeriodByIdHandler : IRequestHandler<GetWalletPeriodByIdQuery, WalletPeriodDto?>
{
    private readonly IWalletPeriodRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetWalletPeriodByIdHandler> _logger;

    public GetWalletPeriodByIdHandler(
        IWalletPeriodRepository repository,
        IMapper mapper,
        ILogger<GetWalletPeriodByIdHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WalletPeriodDto?> Handle(GetWalletPeriodByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWalletPeriodById(request.Id);
        return entity is null ? null : _mapper.Map<WalletPeriodDto>(entity);
    }
}
