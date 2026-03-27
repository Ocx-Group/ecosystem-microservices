using AutoMapper;
using Ecosystem.WalletService.Application.Queries.ResultsEcoPool;
using Ecosystem.WalletService.Domain.DTOs.ResultsEcoPoolDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.ResultsEcoPool;

public class GetAllResultsEcoPoolHandler : IRequestHandler<GetAllResultsEcoPoolQuery, ICollection<ResultsEcoPoolDto>>
{
    private readonly IResultsEcoPoolRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllResultsEcoPoolHandler> _logger;

    public GetAllResultsEcoPoolHandler(
        IResultsEcoPoolRepository repository,
        IMapper mapper,
        ILogger<GetAllResultsEcoPoolHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<ResultsEcoPoolDto>> Handle(GetAllResultsEcoPoolQuery request, CancellationToken cancellationToken)
    {
        var results = await _repository.GetAllResultsEcoPool();
        return _mapper.Map<ICollection<ResultsEcoPoolDto>>(results);
    }
}
