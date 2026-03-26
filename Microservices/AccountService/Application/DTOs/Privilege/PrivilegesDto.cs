namespace Ecosystem.AccountService.Application.DTOs.Privilege;

public class PrivilegesDto
{
    public int Id { get; set; }
    public int RolId { get; set; }
    public int MenuConfigurationId { get; set; }
    public bool CanCreate { get; set; }
    public bool CanRead { get; set; }
    public bool CanDelete { get; set; }
    public bool CanEdit { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
