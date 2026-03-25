namespace Ecosystem.Domain.Core.Configuration;

public class CookieConfig
{
    public const string SectionName = "CookieSettings";
    
    public string? Domain { get; set; }
    public bool SecureOnly { get; set; } = true;
    public string SameSite { get; set; } = "Strict";
    public string AccessTokenCookieName { get; set; } = "access-token";
    public string RefreshTokenCookieName { get; set; } = "refresh-token";
    public string Path { get; set; } = "/";
}