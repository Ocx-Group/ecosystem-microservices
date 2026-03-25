using Ecosystem.AccountService.Domain.Models;
using Ecosystem.AccountService.Domain.Models.CustomModels;

namespace Ecosystem.AccountService.Domain.Interfaces;

public interface IMatrixPositionsRepository
{
    Task<List<MatrixTree>> GetMatrixTreeAsync(int rootPositionId, int matrixType);
    Task<MatrixPosition?> GetByUserAndMatrixTypeAsync(int userId, int matrixType);
    Task<int> CountByMatrixTypeAsync(int matrixType);
    Task<List<MatrixPosition>> GetAllByMatrixTypeAsync(int matrixType);
    Task<MatrixPosition?> CreateAsync(MatrixPosition position);
    Task<IEnumerable<MatrixPosition>> GetPositionsByUserIdAsync(int userId);
    Task<MatrixPosition?> GetByIdAsync(int positionId);
    Task BulkCreateAsync(IEnumerable<MatrixPosition> positions);
    Task<List<MatrixPosition>> GetManyByUsersAndMatrixTypeAsync(IReadOnlyCollection<int> userIds, int matrixType);
}
