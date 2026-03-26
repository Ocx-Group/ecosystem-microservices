namespace Ecosystem.ConfigurationService.Application.DTOs;

public class GeneralConfigurationDto
{
    public DateTime PaymentModelCutoffDate { get; set; }
    public bool IsUnderMaintenance { get; set; }
}
