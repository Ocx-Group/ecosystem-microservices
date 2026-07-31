using System.Security.Claims;
using Ecosystem.AccountService.Application.Services;
using Ecosystem.AccountService.Application.Settings;
using Ecosystem.AccountService.Domain.Models;
using Microsoft.Extensions.Options;
using Xunit;

namespace Ecosystem.AccountService.AuthenticationTests;

public sealed class AdminAccessTokenIssuerTests
{
    [Fact]
    public void Issue_ContainsSignedRoleAndTenantClaims()
    {
        var issuer = new AdminAccessTokenIssuer(Options.Create(new JwtSettings
        {
            Key = "unit-test-signing-key-with-at-least-32-bytes",
            Issuer = "Ecosystem.Tests",
            Audience = "Ecosystem.Tests",
            AdminTokenLifetimeMinutes = 15
        }));
        var user = new User
        {
            Id = 42,
            Username = "admin",
            BrandId = 7,
            Rol = new Role { Name = "Administrador" }
        };

        var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler()
            .ReadJwtToken(issuer.Issue(user));

        Assert.Equal("7", token.Claims.Single(claim => claim.Type == "brand_id").Value);
        Assert.Equal("admin", token.Claims.Single(claim => claim.Type == "token_type").Value);
        Assert.Contains(token.Claims, claim =>
            claim.Type == ClaimTypes.Role && claim.Value == "Administrador");
    }

    [Fact]
    public void Issue_RejectsPlaceholderSigningKey()
    {
        var issuer = new AdminAccessTokenIssuer(Options.Create(new JwtSettings
        {
            Key = "CHANGE_ME_USE_ENVIRONMENT_VARIABLE",
            Issuer = "Ecosystem",
            Audience = "Ecosystem"
        }));

        Assert.Throws<InvalidOperationException>(() => issuer.Issue(new User
        {
            Id = 1,
            Username = "admin",
            BrandId = 1,
            Rol = new Role { Name = "Administrador" }
        }));
    }
}
