using Ecosystem.WalletService.Domain.Requests.MatrixRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.MatrixQualification;

public record ProcessDirectPaymentMatrixCommand : IRequest<bool>
{
    public int UserId { get; init; }
    public int MatrixType { get; init; }
    public int? RecipientId { get; init; }
    public int? Cycle { get; init; }
}
