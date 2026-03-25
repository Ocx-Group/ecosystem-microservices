using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Ecosystem.AccountService.Domain.Models.CustomModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace Ecosystem.AccountService.Data.Repositories;

public class MatrixPositionsRepository(AccountServiceDbContext context)
    : BaseRepository(context), IMatrixPositionsRepository
{
    public async Task<List<MatrixTree>> GetMatrixTreeAsync(int userId, int matrixType)
    {
        const int unlimited = int.MaxValue;
        return await Context.Set<MatrixTree>()
            .FromSqlInterpolated($@"
            SELECT *
            FROM account_service.get_matrix_tree_by_user(
                {userId}, {matrixType}, {unlimited}, {unlimited}
            )")
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<MatrixPosition?> GetByUserAndMatrixTypeAsync(int userId, int matrixType)
        => await Context.MatrixPositions.FirstOrDefaultAsync(x => x.UserId == userId && x.MatrixType == matrixType);

    public async Task<int> CountByMatrixTypeAsync(int matrixType)
        => await Context.MatrixPositions.CountAsync(x => x.MatrixType == matrixType && x.DeletedAt == null);

    public async Task<List<MatrixPosition>> GetAllByMatrixTypeAsync(int matrixType)
        => await Context.MatrixPositions
            .Where(x => x.MatrixType == matrixType && x.DeletedAt == null)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();

    public async Task<MatrixPosition?> CreateAsync(MatrixPosition position)
    {
        var today = DateTime.Now;
        position.CreatedAt = today;
        position.UpdatedAt = today;
        await Context.MatrixPositions.AddAsync(position);
        await Context.SaveChangesAsync();
        return position;
    }

    public async Task<IEnumerable<MatrixPosition>> GetPositionsByUserIdAsync(int userId)
    {
        return await Context.MatrixPositions
            .Where(p => p.UserId == userId && p.DeletedAt == null)
            .ToListAsync();
    }

    public async Task<MatrixPosition?> GetByIdAsync(int positionId)
    {
        return await Context.MatrixPositions
            .FirstOrDefaultAsync(p => p.PositionId == positionId && p.DeletedAt == null);
    }

    public async Task BulkCreateAsync(IEnumerable<MatrixPosition> positions)
    {
        var now = DateTime.UtcNow;
        var rows = positions.Select(p => new
        {
            p.UserId,
            p.MatrixType,
            ParentId = (int?)p.ParentPositionId,
            Created = now,
            Updated = now
        }).ToList();

        if (rows.Count == 0) return;

        await using var conn = (NpgsqlConnection)Context.Database.GetDbConnection();
        await conn.OpenAsync();
        await using var tx = await conn.BeginTransactionAsync();

        const string copyCmd = @"
        COPY account_service.matrix_positions
             (user_id, matrix_type, parent_position_id, created_at, updated_at)
        FROM STDIN (FORMAT binary);";

        await using (var writer = await conn.BeginBinaryImportAsync(copyCmd))
        {
            foreach (var r in rows)
            {
                await writer.StartRowAsync();
                await writer.WriteAsync(r.UserId, NpgsqlDbType.Integer);
                await writer.WriteAsync((short)r.MatrixType, NpgsqlDbType.Smallint);
                if (r.ParentId.HasValue)
                    await writer.WriteAsync(r.ParentId.Value, NpgsqlDbType.Integer);
                else
                    await writer.WriteNullAsync();
                await writer.WriteAsync(r.Created, NpgsqlDbType.Timestamp);
                await writer.WriteAsync(r.Updated, NpgsqlDbType.Timestamp);
            }
            await writer.CompleteAsync();
        }
        await tx.CommitAsync();
    }

    public async Task<List<MatrixPosition>> GetManyByUsersAndMatrixTypeAsync(IReadOnlyCollection<int> userIds, int matrixType)
    {
        if (userIds.Count == 0) return [];

        var idsParam = new NpgsqlParameter<int[]>("ids", userIds.ToArray()) { NpgsqlDbType = NpgsqlDbType.Array | NpgsqlDbType.Integer };
        var typeParam = new NpgsqlParameter<short>("matrixType", (short)matrixType);

        const string sql = @"
             SELECT *
             FROM   account_service.matrix_positions
             WHERE  deleted_at IS NULL
               AND  matrix_type = @matrixType
               AND  user_id     = ANY(@ids)";

        return await Context.MatrixPositions
            .FromSqlRaw(sql, typeParam, idsParam)
            .AsNoTracking()
            .ToListAsync();
    }
}
