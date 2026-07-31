using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecosystem.AccountService.Application.Settings;
using Ecosystem.AccountService.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ecosystem.AccountService.Application.Services;

public sealed class AdminAccessTokenIssuer(IOptions<JwtSettings> options)
    : IAdminAccessTokenIssuer
{
    private const string BrandIdClaim = "brand_id";
    private const string TokenTypeClaim = "token_type";
    private readonly JwtSettings _settings = options.Value;

    public string Issue(User user)
    {
        ValidateSettings();

        var now = DateTime.UtcNow;
        var roleName = user.Rol?.Name ?? string.Empty;
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, roleName),
            new(BrandIdClaim, user.BrandId.ToString()),
            new(TokenTypeClaim, "admin")
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(NormalizeLifetime(_settings.AdminTokenLifetimeMinutes)),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private void ValidateSettings()
    {
        if (string.IsNullOrWhiteSpace(_settings.Key) ||
            _settings.Key.StartsWith("CHANGE_ME", StringComparison.OrdinalIgnoreCase) ||
            Encoding.UTF8.GetByteCount(_settings.Key) < 32)
        {
            throw new InvalidOperationException(
                "Jwt:Key must be supplied from a secret and contain at least 32 bytes.");
        }

        if (string.IsNullOrWhiteSpace(_settings.Issuer) ||
            string.IsNullOrWhiteSpace(_settings.Audience))
        {
            throw new InvalidOperationException("Jwt issuer and audience are required.");
        }
    }

    private static int NormalizeLifetime(int configuredMinutes)
        => Math.Clamp(configuredMinutes, 5, 60);
}
