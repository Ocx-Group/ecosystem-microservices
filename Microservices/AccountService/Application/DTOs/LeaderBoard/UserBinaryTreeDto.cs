namespace Ecosystem.AccountService.Application.DTOs.LeaderBoard;

public class UserBinaryTreeDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int Side { get; set; }
    public int Level { get; set; }
    public int Father { get; set; }
    public decimal Amount { get; set; }
    public List<UserBinaryTreeDto> Children { get; set; } = [];
}
