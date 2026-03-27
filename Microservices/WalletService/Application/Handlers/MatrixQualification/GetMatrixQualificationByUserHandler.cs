using AutoMapper;
using Ecosystem.WalletService.Application.Queries.MatrixQualification;
using Ecosystem.WalletService.Domain.DTOs.MatrixQualificationDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.MatrixQualification;

public class GetMatrixQualificationByUserHandler : IRequestHandler<GetMatrixQualificationByUserQuery, MatrixQualificationDto?>
{
    private readonly IMatrixQualificationRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetMatrixQualificationByUserHandler> _logger;

    public GetMatrixQualificationByUserHandler(
        IMatrixQualificationRepository repository,
        IMapper mapper,
        ILogger<GetMatrixQualificationByUserHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<MatrixQualificationDto?> Handle(GetMatrixQualificationByUserQuery request, CancellationToken cancellationToken)
    {
        var qualification = await _repository.GetByUserAndMatrixTypeAsync(request.UserId, request.MatrixType);
        if (qualification is null)
            throw new ApplicationException($"Qualification for user {request.UserId} and matrix type {request.MatrixType} not found.");

        return _mapper.Map<MatrixQualificationDto>(qualification);
    }
}
