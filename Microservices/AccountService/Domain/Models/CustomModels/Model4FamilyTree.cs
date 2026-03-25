using System.ComponentModel.DataAnnotations.Schema;

namespace Ecosystem.AccountService.Domain.Models.CustomModels;

public class ModelsFamilyTree
{
    [Column("id")]
    public int Id { get; set; }
    [Column("user_name")]
    public string UserName { get; set; }
    [Column("side")]
    public int Side { get; set; }
    [Column("amount")]
    public decimal Amount { get; set; }
    [Column("level")]
    public int Level { get; set; }
    [Column("father")]
    public int? Father { get; set; }
}