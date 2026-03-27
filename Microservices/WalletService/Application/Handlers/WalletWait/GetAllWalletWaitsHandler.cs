using AutoMapper;
using Ecosystem.WalletService.Application.Queries.WalletWait;
using Ecosystem.WalletService.Domain.DTOs.WalletWaitDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletWait;

public class GetAllWalletWaitsHandler : IRequestHandler<GetAllWalletWaitsQuery, ICollection<WalletWaitDto>>
{
    private readonly IWalletWaitRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllWalletWaitsHandler> _logger;

    public GetAllWalletWaitsHandler(
        IWalletWaitRepository repository,
        IMapper mapper,
        ILogger<GetAllWalletWaitsHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<WalletWaitDto>> Handle(GetAllWalletWaitsQuery request, CancellationToken cancellationToken)
    {
        var waits = await _repository.GetAllWalletsWaits();
        return _mapper.Map<ICollection<WalletWaitDto>>(waits);
    }
}
