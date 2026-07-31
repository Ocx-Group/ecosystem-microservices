namespace Ecosystem.AccountService.Application.Settings;

public sealed class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Key { get; init; } = string.Empty;
    public string Issuer { get; init; } = "Ecosystem";
    public string Audience { get; init; } = "Ecosystem";
    public int AdminTokenLifetimeMinutes { get; init; } = 15;
}
