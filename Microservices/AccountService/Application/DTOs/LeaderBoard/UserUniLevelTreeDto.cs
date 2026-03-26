namespace Ecosystem.AccountService.Application.DTOs.LeaderBoard;

public class UserUniLevelTreeDto
{
    public int id { get; set; }
    public string userName { get; set; } = string.Empty;
    public int level { get; set; }
    public int father { get; set; }
    public int? grading { get; set; }
    public string? imageProfileUrl { get; set; }
    public List<UserUniLevelTreeDto> children { get; set; } = [];
}
