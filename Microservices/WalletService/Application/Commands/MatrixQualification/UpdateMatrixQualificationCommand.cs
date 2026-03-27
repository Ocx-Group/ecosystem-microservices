using Ecosystem.WalletService.Domain.DTOs.MatrixQualificationDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.MatrixQualification;

public record UpdateMatrixQualificationCommand : IRequest<MatrixQualificationDto?>
{
    public int QualificationId { get; init; }
    public long UserId { get; init; }
    public int MatrixType { get; init; }
    public decimal TotalEarnings { get; init; }
    public decimal WithdrawnAmount { get; init; }
    public decimal AvailableBalance { get; init; }
    public bool IsQualified { get; init; }
}
