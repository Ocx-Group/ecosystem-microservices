using AutoMapper;
using Ecosystem.WalletService.Application.Queries.WalletPeriod;
using Ecosystem.WalletService.Domain.DTOs.WalletPeriodDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletPeriod;

public class GetAllWalletPeriodsHandler : IRequestHandler<GetAllWalletPeriodsQuery, ICollection<WalletPeriodDto>>
{
    private readonly IWalletPeriodRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllWalletPeriodsHandler> _logger;

    public GetAllWalletPeriodsHandler(
        IWalletPeriodRepository repository,
        IMapper mapper,
        ILogger<GetAllWalletPeriodsHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<WalletPeriodDto>> Handle(GetAllWalletPeriodsQuery request, CancellationToken cancellationToken)
    {
        var periods = await _repository.GetAllWalletsPeriods();
        return _mapper.Map<ICollection<WalletPeriodDto>>(periods);
    }
}
