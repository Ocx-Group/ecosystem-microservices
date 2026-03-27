using MediatR;

namespace Ecosystem.WalletService.Application.Commands.MatrixQualification;

public record GetAllInconsistentRecordsCommand : IRequest<string>;
