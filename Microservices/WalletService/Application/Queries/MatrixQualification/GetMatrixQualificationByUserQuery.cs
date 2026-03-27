using Ecosystem.WalletService.Domain.DTOs.MatrixQualificationDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.MatrixQualification;

public record GetMatrixQualificationByUserQuery(long UserId, int MatrixType) : IRequest<MatrixQualificationDto?>;
