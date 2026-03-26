namespace Ecosystem.ConfigurationService.Domain.Models;

public partial class Incentives
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int ScopeLevel { get; set; }
    public bool IsInfinity { get; set; }
    public decimal PersonalPurchases { get; set; }
    public bool PersonalPurchasesExact { get; set; }
    public decimal PurchasesNetwork { get; set; }
    public decimal BinaryVolume { get; set; }
    public int VolumePoints { get; set; }
    public int VolumePointsNetwork { get; set; }
    public int ChildrenLeftLeg { get; set; }
    public int ChildrenRightLeg { get; set; }
    public int FrontByMatrix { get; set; }
    public int FrontQualif1 { get; set; }
    public int FrontScore1 { get; set; }
    public int FrontQuali2 { get; set; }
    public int FrontScore2 { get; set; }
    public int FrontQualif3 { get; set; }
    public int FrontScore3 { get; set; }
    public bool ExactFrontRatings { get; set; }
    public int LeaderByMatrix { get; set; }
    public int? NetworkLeaders { get; set; }
    public int? NetworkLeadersQualifier { get; set; }
    public int? Products { get; set; }
    public int? Affiliations { get; set; }
    public int? Grading { get; set; }
    public int Active { get; set; }
    public bool? Status { get; set; }
    public long? BrandId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
