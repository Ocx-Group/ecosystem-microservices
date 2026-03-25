using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.AccountService.Data.Repositories;

public class MatrixCyclesRepository(AccountServiceDbContext context) : BaseRepository(context), IMatrixCyclesRepository
{
    public async Task<List<MatrixCycle>> GetCompletedCyclesAsync(int matrixType, int pageNumber, int pageSize)
    {
        return await Context.MatrixCycles
            .Where(x => x.MatrixType == matrixType && x.IsCompleted)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<MatrixCycle?> GetCurrentCycleAsync(int initiatorUserId, int matrixType)
    {
        return await Context.MatrixCycles
            .Where(x => x.InitiatorUserId == initiatorUserId && x.MatrixType == matrixType && !x.IsCompleted)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<MatrixCycle?> CreateAsync(MatrixCycle matrixCycle)
    {
        var today = DateTime.UtcNow;
        matrixCycle.CreatedAt = today;
        matrixCycle.UpdatedAt = today;
        await Context.MatrixCycles.AddAsync(matrixCycle);
        await Context.SaveChangesAsync();
        return matrixCycle;
    }

    public async Task<MatrixCycle?> UpdateAsync(MatrixCycle matrixCycle)
    {
        var today = DateTime.UtcNow;
        matrixCycle.UpdatedAt = today;
        Context.MatrixCycles.Update(matrixCycle);
        await Context.SaveChangesAsync();
        return matrixCycle;
    }

    public async Task<MatrixCycle?> GetByIdAsync(int cycleId)
        => await Context.MatrixCycles.FirstOrDefaultAsync(x => x.CycleId == cycleId);
}
