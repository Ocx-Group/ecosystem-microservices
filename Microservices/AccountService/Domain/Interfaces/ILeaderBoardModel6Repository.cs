using Ecosystem.AccountService.Domain.Models;
using Ecosystem.AccountService.Domain.Models.CustomModels;

namespace Ecosystem.AccountService.Domain.Interfaces;

public interface ILeaderBoardModel6Repository
{
    Task<List<ModelsFamilyTree>> GetTreeModel6ByUser(int? userId);
    Task AddLeaderBoard(ICollection<LeaderBoardModel6> treeModel4);
    Task DeleteLeaderBoard();
    Task<int> CountDirectUsers(int userId);
    Task<int> CountInDirectUsers();
}
