using Microsoft.EntityFrameworkCore.Storage;
using Ecosystem.WalletService.Domain.Models;

namespace Ecosystem.WalletService.Domain.Interfaces;

public interface IMatrixEarningsRepository
{
    Task<decimal> GetTotalEarningsAsync(long userId, int matrixType);
    Task<MatrixEarning?> CreateAsync(MatrixEarning matrixEarning,int qualificationCount = 0);
    Task<IDbContextTransaction> BeginTransactionAsync();
}