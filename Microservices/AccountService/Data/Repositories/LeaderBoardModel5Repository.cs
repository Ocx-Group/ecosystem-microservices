using System.Runtime.CompilerServices;
using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Ecosystem.AccountService.Domain.Models.CustomModels;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.AccountService.Data.Repositories;

public class LeaderBoardModel5Repository : BaseRepository, ILeaderBoardModel5Repository
{
    public LeaderBoardModel5Repository(AccountServiceDbContext context) : base(context) { }

    public Task<List<ModelsFamilyTree>> TreeModel5ByUser(int? userId)
    {
        int id = userId ?? 0;
        int maxLevel = 8;
        bool isAdmin = false;
        return Context.Set<ModelsFamilyTree>()
            .FromSqlRaw("SELECT * FROM get_model5_family_tree(@p0, @p1, @p2)", id, maxLevel, isAdmin)
            .ToListAsync();
    }

    public Task AddLeaderBoard(ICollection<LeaderBoardModel5> treeModel5)
    {
        Context.LeaderBoardModel5s.AddRangeAsync(treeModel5);
        return Context.SaveChangesAsync();
    }

    public Task DeleteLeaderBoard()
    {
        var sql = FormattableStringFactory.Create("TRUNCATE TABLE account_service.leader_board_model5");
        return Context.Database.ExecuteSqlInterpolatedAsync(sql);
    }

    public Task<int> CountDirectUsers(int userId)
    {
        return Context.LeaderBoardModel5s.Where(x => x.FatherModel5 == userId).CountAsync();
    }

    public Task<int> CountInDirectUsers()
    {
        return Context.LeaderBoardModel5s.CountAsync();
    }
}
