using Ecosystem.AccountService.Application.DTOs.Matrix;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Matrix;

public record GetMatrixTreeQuery(int UserId, int MatrixType) : IRequest<MatrixDto?>;
