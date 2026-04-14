using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Commands.Matrix;

public record PlaceUserInMatrixCommand(int UserId, int MatrixType, int? RecipientId, int? Cycle) : IRequest<bool>;

public class PlaceUserInMatrixHandler : IRequestHandler<PlaceUserInMatrixCommand, bool>
{
    private readonly IMatrixPositionsRepository _matrixPositionsRepository;
    private readonly ILogger<PlaceUserInMatrixHandler> _logger;

    public PlaceUserInMatrixHandler(
        IMatrixPositionsRepository matrixPositionsRepository,
        ILogger<PlaceUserInMatrixHandler> logger)
    {
        _matrixPositionsRepository = matrixPositionsRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(PlaceUserInMatrixCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var position = new MatrixPosition
            {
                UserId = request.UserId,
                MatrixType = request.MatrixType,
                ParentPositionId = request.RecipientId ?? 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await _matrixPositionsRepository.CreateAsync(position);
            return result is not null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error placing user {UserId} in matrix type {MatrixType}",
                request.UserId, request.MatrixType);
            return false;
        }
    }
}