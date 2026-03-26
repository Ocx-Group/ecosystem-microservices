namespace Ecosystem.AccountService.Application.DTOs.Matrix;

public class MatrixDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public int Father { get; set; }
    public int Level { get; set; }
    public string? ImageProfileUrl { get; set; }
    public int? QualificationCount { get; set; }
    public List<MatrixDto> Children { get; set; } = [];
}
