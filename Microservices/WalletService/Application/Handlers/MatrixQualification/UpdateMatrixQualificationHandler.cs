using AutoMapper;
using Ecosystem.WalletService.Application.Commands.MatrixQualification;
using Ecosystem.WalletService.Domain.DTOs.MatrixQualificationDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.MatrixQualification;

public class UpdateMatrixQualificationHandler : IRequestHandler<UpdateMatrixQualificationCommand, MatrixQualificationDto?>
{
    private readonly IMatrixQualificationRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateMatrixQualificationHandler> _logger;

    public UpdateMatrixQualificationHandler(
        IMatrixQualificationRepository repository,
        IMapper mapper,
        ILogger<UpdateMatrixQualificationHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<MatrixQualificationDto?> Handle(UpdateMatrixQualificationCommand request, CancellationToken cancellationToken)
    {
        var qualification = await _repository.GetQualificationById(request.QualificationId);
        if (qualification is null)
            throw new ApplicationException($"Qualification with ID {request.QualificationId} not found.");

        qualification.UserId = request.UserId;
        qualification.MatrixType = request.MatrixType;
        qualification.TotalEarnings = request.TotalEarnings;
        qualification.WithdrawnAmount = request.WithdrawnAmount;
        qualification.AvailableBalance = request.AvailableBalance;
        qualification.IsQualified = request.IsQualified;
        qualification.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(qualification);
        return _mapper.Map<MatrixQualificationDto>(qualification);
    }
}
