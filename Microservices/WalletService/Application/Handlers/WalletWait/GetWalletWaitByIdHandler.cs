using AutoMapper;
using Ecosystem.WalletService.Application.Queries.WalletWait;
using Ecosystem.WalletService.Domain.DTOs.WalletWaitDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletWait;

public class GetWalletWaitByIdHandler : IRequestHandler<GetWalletWaitByIdQuery, WalletWaitDto?>
{
    private readonly IWalletWaitRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetWalletWaitByIdHandler> _logger;

    public GetWalletWaitByIdHandler(
        IWalletWaitRepository repository,
        IMapper mapper,
        ILogger<GetWalletWaitByIdHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WalletWaitDto?> Handle(GetWalletWaitByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWalletWaitById(request.Id);
        return entity is null ? null : _mapper.Map<WalletWaitDto>(entity);
    }
}
