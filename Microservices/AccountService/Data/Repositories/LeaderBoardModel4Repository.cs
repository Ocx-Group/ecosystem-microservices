using System.Runtime.CompilerServices;
using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Ecosystem.AccountService.Domain.Models.CustomModels;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.AccountService.Data.Repositories;

public class LeaderBoardModel4Repository : BaseRepository, ILeaderBoardModel4Repository
{
    public LeaderBoardModel4Repository(AccountServiceDbContext context) : base(context) { }

    public Task<List<ModelsFamilyTree>> GetTreeModel4ByUser(int maxLevels, sbyte isAdmin, int id = 0)
    {
        bool isAdminBool = isAdmin != 0;
        var sql = $"SELECT * FROM account_service.get_model4_family_tree({id}, {maxLevels}, '{isAdminBool.ToString().ToLower()}')";
        return Context.Set<ModelsFamilyTree>().FromSqlRaw(sql).ToListAsync();
    }

    public Task<LeaderBoardModel4?> GetChild(int userId, int side)
    {
        return Context.LeaderBoardModel4s.FirstOrDefaultAsync(x => x.FatherModel4 == userId && x.PositionX == side);
    }

    public Task AddLeaderBoard(ICollection<LeaderBoardModel4> treeModel4)
    {
        Context.LeaderBoardModel4s.AddRangeAsync(treeModel4);
        return Context.SaveChangesAsync();
    }

    public Task DeleteLeaderBoard()
    {
        var sql = FormattableStringFactory.Create("TRUNCATE TABLE account_service.leader_board_model4");
        return Context.Database.ExecuteSqlInterpolatedAsync(sql);
    }
}
