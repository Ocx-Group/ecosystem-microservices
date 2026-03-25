using Ecosystem.AccountService.Domain.Models;

namespace Ecosystem.AccountService.Domain.Interfaces;

public interface IMatrixCyclesRepository
{
    Task<List<MatrixCycle>> GetCompletedCyclesAsync(int matrixType, int pageNumber, int pageSize);
    Task<MatrixCycle?> GetCurrentCycleAsync(int initiatorUserId, int matrixType);
    Task<MatrixCycle?> CreateAsync(MatrixCycle matrixCycle);
    Task<MatrixCycle?> UpdateAsync(MatrixCycle matrixCycle);
    Task<MatrixCycle?> GetByIdAsync(int cycleId);
}
