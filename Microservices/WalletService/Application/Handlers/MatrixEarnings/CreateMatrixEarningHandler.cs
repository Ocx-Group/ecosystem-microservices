using AutoMapper;
using Ecosystem.WalletService.Application.Commands.MatrixEarnings;
using Ecosystem.WalletService.Domain.DTOs.MatrixEarningDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.MatrixEarnings;

public class CreateMatrixEarningHandler : IRequestHandler<CreateMatrixEarningCommand, MatrixEarningDto>
{
    private readonly IMatrixEarningsRepository _matrixEarningsRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateMatrixEarningHandler> _logger;

    public CreateMatrixEarningHandler(
        IMatrixEarningsRepository matrixEarningsRepository,
        IMapper mapper,
        ILogger<CreateMatrixEarningHandler> logger)
    {
        _matrixEarningsRepository = matrixEarningsRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<MatrixEarningDto> Handle(CreateMatrixEarningCommand request, CancellationToken cancellationToken)
    {
        var matrixEarning = _mapper.Map<MatrixEarning>(request);
        var created = await _matrixEarningsRepository.CreateAsync(matrixEarning);
        return _mapper.Map<MatrixEarningDto>(created);
    }
}
