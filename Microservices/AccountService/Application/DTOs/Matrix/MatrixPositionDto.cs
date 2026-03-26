namespace Ecosystem.AccountService.Application.DTOs.Matrix;

public class MatrixPositionDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int MatrixType { get; set; }
    public string MatrixName { get; set; } = string.Empty;
    public int Level { get; set; }
    public DateTime CreatedAt { get; set; }
}
