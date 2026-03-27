namespace Ecosystem.WalletService.Domain.Responses;

public class MatrixPositionResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public bool? Data { get; set; }
    public int? Code { get; set; }
}

public class MatrixPositionDto
{
    public int PositionId { get; set; }
    public long UserId { get; set; }
    public string? UserName { get; set; }
    public string? MatrixName { get; set; }
    public int Level { get; set; }
    public int MatrixType { get; set; }
    public int ParentPositionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}