using System.Runtime.CompilerServices;
using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Ecosystem.AccountService.Domain.Models.CustomModels;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.AccountService.Data.Repositories;

public class LeaderBoardModel6Repository : BaseRepository, ILeaderBoardModel6Repository
{
    public LeaderBoardModel6Repository(AccountServiceDbContext context) : base(context) { }

    public Task<List<ModelsFamilyTree>> GetTreeModel6ByUser(int? userId)
    {
        var sql = FormattableStringFactory.Create(
            $"EXEC account_service.get_model5_family_tree @id = {{0}}, @maxLevel = {{1}}, @isAdmin = {{2}}", userId, 8, 0);
        return Context.Set<ModelsFamilyTree>().FromSqlInterpolated(sql).ToListAsync();
    }

    public Task AddLeaderBoard(ICollection<LeaderBoardModel6> treeModel6)
    {
        Context.LeaderBoardModel6s.AddRangeAsync(treeModel6);
        return Context.SaveChangesAsync();
    }

    public Task DeleteLeaderBoard()
    {
        var sql = FormattableStringFactory.Create("TRUNCATE TABLE account_service.leader_board_model6");
        return Context.Database.ExecuteSqlInterpolatedAsync(sql);
    }

    public Task<int> CountDirectUsers(int userId)
    {
        return Context.LeaderBoardModel6s.CountAsync(x => x.FatherModel6 == userId);
    }

    public Task<int> CountInDirectUsers()
    {
        return Context.LeaderBoardModel6s.CountAsync();
    }
}
