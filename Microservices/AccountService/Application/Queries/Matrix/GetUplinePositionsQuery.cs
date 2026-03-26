using Ecosystem.AccountService.Application.DTOs.Matrix;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Matrix;

public record GetUplinePositionsQuery(int UserId, int MatrixType, int Cycle) : IRequest<IEnumerable<MatrixPositionDto>?>;
