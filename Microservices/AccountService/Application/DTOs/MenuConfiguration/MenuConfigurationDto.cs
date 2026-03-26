namespace Ecosystem.AccountService.Application.DTOs.MenuConfiguration;

public class MenuConfigurationDto
{
    public int Id { get; set; }
    public string MenuName { get; set; } = string.Empty;
    public string PageName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
