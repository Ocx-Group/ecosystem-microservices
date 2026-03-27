using System.Text.Json.Serialization;

namespace Ecosystem.WalletService.Domain.Responses;

public class GetAccountsEcoPoolResponse
{
    public bool success { get; set; }
    public List<UserModelResponse> data { get; set; }
    public string message { get; set; }
    public int code { get; set; }
}

public class UserModelResponse
{
    public int Id { get; set; }
    
    public string UserName { get; set; }
    
    public string Email { get; set; }
    
    public string? Name { get; set; }
    
    public string? LastName { get; set; }
    
    public int Level { get; set; }
    
    public int Father { get; set; }
    
    public int Side { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public List<UserLevel> FamilyTree { get; set; }
}

public class UserLevel
{
    public int Id { get; set; }
    
    public string UserName { get; set; }
    
    public string Email { get; set; }
    
    public string? Name { get; set; }
    
    public string? LastName { get; set; }
    
    public int Level { get; set; }
    
    public int Father { get; set; }
    
    public int Side { get; set; }
    public DateTime CreatedAt { get; set; }
}