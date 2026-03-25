namespace Ecosystem.AccountService.Domain.Models;

public class MatrixPosition
{
    public int PositionId { get; set; }
    public long UserId { get; set; }
    public int MatrixType { get; set; }
    public int ParentPositionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public UsersAffiliate? User { get; set; }
}
