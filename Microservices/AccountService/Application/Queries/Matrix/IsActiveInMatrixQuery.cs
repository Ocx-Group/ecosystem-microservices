using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Matrix;

public record IsActiveInMatrixQuery(int UserId, int MatrixType, int Cycle) : IRequest<bool>;
