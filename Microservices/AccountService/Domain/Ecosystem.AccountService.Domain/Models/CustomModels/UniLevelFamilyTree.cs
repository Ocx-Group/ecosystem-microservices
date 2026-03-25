using System.ComponentModel.DataAnnotations.Schema;

namespace Ecosystem.AccountService.Domain.Models.CustomModels;

public class UniLevelFamilyTree
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public int Level { get; set; }
    public int Father { get; set; }
    public int? Grading { get; set; }

    [Column("image_profile_url")]
    public string? ImageProfileUrl { get; set; }
}
