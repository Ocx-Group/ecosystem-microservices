namespace Ecosystem.ConfigurationService.Application.DTOs;

public class MatrixConfigDto
{
    public int MatrixType { get; set; }
    public decimal Threshold { get; set; }
    public decimal FeeAmount { get; set; }
    public decimal MinWithdraw { get; set; }
    public decimal MaxWithdraw { get; set; }
    public int ChildCount { get; set; }
    public decimal RangeMin { get; set; }
    public decimal RangeMax { get; set; }
    public int Levels { get; set; }
    public string MatrixName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
