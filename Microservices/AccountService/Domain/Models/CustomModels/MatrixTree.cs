using System.ComponentModel.DataAnnotations.Schema;

namespace Ecosystem.AccountService.Domain.Models.CustomModels;

public class MatrixTree
{
    [Column("id")]
    public int Id { get; set; }

    [Column("user_name")]
    public string Username { get; set; }

    [Column("level")]
    public int Level { get; set; }

    [Column("father")]
    public int Father { get; set; }

    [Column("image_profile_url")]
    public string? ImageProfileUrl { get; set; }

    [Column("qualification_count")]
    public int? QualificationCount { get; set; }
}
