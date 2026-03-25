using Ecosystem.AccountService.Domain.Models;
using Ecosystem.AccountService.Domain.Models.CustomModels;

namespace Ecosystem.AccountService.Domain.Interfaces;

public interface ILeaderBoardModel5Repository
{
    Task<List<ModelsFamilyTree>> TreeModel5ByUser(int? userId);
    Task AddLeaderBoard(ICollection<LeaderBoardModel5> treeModel4);
    Task DeleteLeaderBoard();
    Task<int> CountDirectUsers(int userId);
    Task<int> CountInDirectUsers();
}
