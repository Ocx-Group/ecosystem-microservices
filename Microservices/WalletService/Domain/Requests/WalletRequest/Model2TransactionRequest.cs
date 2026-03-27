namespace Ecosystem.WalletService.Domain.Requests.WalletRequest;
public class Model2TransactionRequest
{
    public int Case { get; set; }
    public decimal Points { get; set; }
    public decimal EcoPoolPercentage { get; set; }
    public decimal CompanyPercentage { get; set; }
    public decimal CompanyPercentageLevels { get; set; }
    public int EcoPoolConfigurationId { get; set; }
    public double TotalPercentageLevels { get; set; }
    public ICollection<Model2Type> EcoPoolsType { get; set; }
    public ICollection<Model2LevelsType> LevelsType { get; set; }
    
}

public class Model2LevelsType
{
    public int Level { get; set; }
    public double Percentage { get; set; }
    public int AffiliateId { get; set; }
    public string AffiliateName { get; set; }
    public int PoolId { get; set; }
    public int Side { get; set; }
    public DateTime? UserCreatedAt { get; set; }
    
}

public class Model2Type
{
    public int AffiliateId { get; set; }
    public int ProductExternalId { get; set; }
    public string AffiliateUserName { get; set; }
    public string ProductName { get; set; }
    public int CountDays { get; set; }
    public int DaysInMonth { get; set; }
    public decimal Amount { get; set; }
    public DateTime LastDayDate { get; set; }
    public DateTime PaymentDate { get; set; }
    public int PoolId { get; set; }
    public DateTime? UserCreatedAt { get; set; }
}