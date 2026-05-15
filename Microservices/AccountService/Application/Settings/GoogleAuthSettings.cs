namespace Ecosystem.AccountService.Application.Settings;

public class GoogleAuthSettings
{
    public const string SectionName = "GoogleAuth";

    public string[] ClientIds { get; set; } = [];
}
