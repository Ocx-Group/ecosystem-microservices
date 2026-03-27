using Ecosystem.WalletService.Domain.DTOs.MatrixEarningDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.MatrixEarnings;

public record CreateMatrixEarningCommand : IRequest<MatrixEarningDto>
{
    public long UserId { get; init; }
    public int MatrixType { get; init; }
    public decimal Amount { get; init; }
    public int SourceUserId { get; init; }
    public string EarningType { get; init; } = null!;
}
