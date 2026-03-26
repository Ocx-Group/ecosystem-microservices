namespace Ecosystem.AccountService.Application.DTOs.Privilege;

public class PrivilegeMenuConfigurationDto
{
    public long? PrivilegeId { get; set; }
    public int? MenuConfigurationId { get; set; }
    public string? MenuName { get; set; }
    public string PageName { get; set; } = string.Empty;
    public bool CanCreate { get; set; }
    public bool CanRead { get; set; }
    public bool CanDelete { get; set; }
    public bool CanEdit { get; set; }
}
