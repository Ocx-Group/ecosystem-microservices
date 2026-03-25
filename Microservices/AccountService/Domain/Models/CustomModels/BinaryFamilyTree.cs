namespace Ecosystem.AccountService.Domain.Models.CustomModels;

public class BinaryFamilyTree
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public byte Side { get; set; }
    public int Level { get; set; }
    public int BinaryFather { get; set; }
}
