using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Interfaces;

namespace Ecosystem.WalletService.Data.Repositories;

public class MatrixEarningsRepository(WalletServiceDbContext context) : BaseRepository(context), IMatrixEarningsRepository
{
    public async Task<decimal> GetTotalEarningsAsync(long userId, int matrixType)
        => await Context.MatrixEarnings.Where(x => x.UserId == userId && x.MatrixType == matrixType)
            .SumAsync(x => x.Amount);

    public async Task<MatrixEarning?> CreateAsync(MatrixEarning matrixEarning, int qualificationCount = 0)
    {
        // Si tenemos el contador de calificaciones, lo pasamos al SP
        await Context.Database.ExecuteSqlRawAsync(
            "CALL wallet_service.sp_process_matrix_commission(@p0, @p1, @p2, @p3, @p4, @p5)",
            matrixEarning.UserId,
            matrixEarning.MatrixType,
            matrixEarning.Amount,
            matrixEarning.SourceUserId,
            matrixEarning.EarningType,
            qualificationCount
        );
    
        return matrixEarning;
    }
    
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await Context.Database.BeginTransactionAsync();
    }
}