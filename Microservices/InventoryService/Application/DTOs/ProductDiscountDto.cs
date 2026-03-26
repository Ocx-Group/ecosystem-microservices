namespace Ecosystem.InventoryService.Application.DTOs;

public class ProductDiscountDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int IdProduct { get; set; }
    public int Qualification { get; set; }
    public decimal Percentage { get; set; }
    public bool? PointsQualify { get; set; }
    public bool? BinaryPoints { get; set; }
    public bool? Commissionable { get; set; }
    public decimal PCommissionable { get; set; }
    public decimal? PBinaryPoints { get; set; }
    public decimal PPointsQualify { get; set; }
    public DateTime Date { get; set; }
}
