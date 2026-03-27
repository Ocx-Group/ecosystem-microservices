using MediatR;

namespace Ecosystem.WalletService.Application.Commands.MatrixQualification;

public record ProcessQualificationAdminCommand(int UserId, int MatrixType) : IRequest<bool>;
