using Ecosystem.AccountService.Domain.Models;
using Ecosystem.AccountService.Domain.Models.CustomModels;

namespace Ecosystem.AccountService.Domain.Interfaces;

public interface ILeaderBoardModel4Repository
{
    Task<List<ModelsFamilyTree>> GetTreeModel4ByUser(int maxLevels, sbyte isAdmin, int id = 0);
    Task<LeaderBoardModel4?> GetChild(int userId, int side);
    Task AddLeaderBoard(ICollection<LeaderBoardModel4> treeModel4);
    Task DeleteLeaderBoard();
}
