using AutoMapper;
using Ecosystem.WalletService.Application.Queries.WalletHistory;
using Ecosystem.WalletService.Domain.DTOs.WalletHistoryDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletHistory;

public class GetWalletHistoryByIdHandler : IRequestHandler<GetWalletHistoryByIdQuery, WalletHistoryDto?>
{
    private readonly IWalletHistoryRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetWalletHistoryByIdHandler> _logger;

    public GetWalletHistoryByIdHandler(
        IWalletHistoryRepository repository,
        IMapper mapper,
        ILogger<GetWalletHistoryByIdHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WalletHistoryDto?> Handle(GetWalletHistoryByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWalletHistoriesByIdAsync(request.Id);
        return entity is null ? null : _mapper.Map<WalletHistoryDto>(entity);
    }
}
