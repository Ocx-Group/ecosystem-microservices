using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.MatrixQualification;

public record ProcessAllQualificationsCommand(int[]? UserIds = null) : IRequest<BatchProcessingResult>;
