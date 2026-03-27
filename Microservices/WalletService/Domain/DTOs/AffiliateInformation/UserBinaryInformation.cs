
using System.Text.Json.Serialization;

namespace Ecosystem.WalletService.Domain.DTOs.AffiliateInformation;

public class UserBinaryInformation
{
    public int AffiliateId { get; set; }
    public decimal LeftVolume { get; set; }
    public decimal RightVolume { get; set; } 
}

public class UserBinaryResponse
{
    public bool success { get; set; }
    
    public List<UserBinaryInformation> data { get; set; }
    
    public string message { get; set; }
    
    public int code { get; set; }
}
