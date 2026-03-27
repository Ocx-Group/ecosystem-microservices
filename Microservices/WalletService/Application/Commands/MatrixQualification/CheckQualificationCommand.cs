using MediatR;

namespace Ecosystem.WalletService.Application.Commands.MatrixQualification;

public record CheckQualificationCommand(int UserId, int MatrixType) : IRequest<bool>;
