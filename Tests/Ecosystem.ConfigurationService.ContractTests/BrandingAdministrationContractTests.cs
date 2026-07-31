using System.ComponentModel.DataAnnotations;
using Ecosystem.ConfigurationService.Application.Commands.BrandConfiguration;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Handlers.BrandConfiguration;
using Ecosystem.ConfigurationService.Api.Controllers;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.BrandConfiguration;
using Ecosystem.Domain.Core.MultiTenancy;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Xunit;
using BrandConfigurationEntity = Ecosystem.ConfigurationService.Domain.Models.BrandConfiguration;

namespace Ecosystem.ConfigurationService.ContractTests;

public sealed class BrandingAdministrationContractTests
{
    [Fact]
    public void UpdateRequest_DoesNotAllowClientSelectedBrandId()
    {
        Assert.Null(typeof(UpdateOwnBrandingRequest).GetProperty("BrandId"));
    }

    [Theory]
    [InlineData(nameof(BrandConfigurationController.GetOwnBranding))]
    [InlineData(nameof(BrandConfigurationController.UpdateOwnBranding))]
    public void DashboardEndpoints_RequireBrandAdministratorPolicy(string methodName)
    {
        var method = typeof(BrandConfigurationController).GetMethod(methodName);
        var authorize = Assert.Single(method!.GetCustomAttributes(
            typeof(AuthorizeAttribute), inherit: true).Cast<AuthorizeAttribute>());

        Assert.Equal("BrandAdministrator", authorize.Policy);
    }

    [Fact]
    public void PublicEndpoint_RemainsExplicitlyAnonymous()
    {
        var method = typeof(BrandConfigurationController)
            .GetMethod(nameof(BrandConfigurationController.GetPublicCurrent));

        Assert.Single(method!.GetCustomAttributes(
            typeof(AllowAnonymousAttribute), inherit: true));
    }

    [Fact]
    public void AdministrationDto_ExposesOnlyBrandingFields()
    {
        var actual = typeof(BrandingAdministrationDto)
            .GetProperties()
            .Select(property => property.Name)
            .OrderBy(name => name)
            .ToArray();

        var expected = new[]
        {
            "BackgroundColor",
            "BrandId",
            "ClientUrl",
            "CompanyIdentifier",
            "CompanyName",
            "DocumentType",
            "LogoUrl",
            "Name",
            "PrimaryColor",
            "SecondaryColor",
            "SupportEmail",
            "SupportPhone",
            "UpdatedAt"
        }.OrderBy(name => name).ToArray();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UpdateRequest_RejectsInvalidUrlsEmailAndColors()
    {
        var request = ValidRequest() with
        {
            ClientUrl = "javascript:alert(1)",
            LogoUrl = "not-a-url",
            SupportEmail = "invalid",
            PrimaryColor = "red"
        };

        var results = Validate(request);

        Assert.Contains(results, result => result.MemberNames.Contains("ClientUrl"));
        Assert.Contains(results, result => result.MemberNames.Contains("LogoUrl"));
        Assert.Contains(results, result => result.MemberNames.Contains("SupportEmail"));
        Assert.Contains(results, result => result.MemberNames.Contains("PrimaryColor"));
    }

    [Fact]
    public async Task UpdateHandler_UsesAuthenticatedTenantAndInvalidatesOnlyItsCache()
    {
        var repository = new RecordingRepository();
        var provider = new RecordingBrandConfigurationProvider();
        var handler = new UpdateOwnBrandingHandler(
            repository,
            provider,
            new FixedTenantContext(7),
            NullLogger<UpdateOwnBrandingHandler>.Instance);

        var result = await handler.Handle(
            new UpdateOwnBrandingCommand(ValidRequest(), "42", "admin"),
            CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(7, repository.UpdatedBrandId);
        Assert.Equal(7, result.BrandId);
        Assert.Equal(7, provider.InvalidatedBrandId);
    }

    [Fact]
    public async Task UpdateHandler_FailsClosedWithoutAuthenticatedTenant()
    {
        var handler = new UpdateOwnBrandingHandler(
            new RecordingRepository(),
            new RecordingBrandConfigurationProvider(),
            new FixedTenantContext(0),
            NullLogger<UpdateOwnBrandingHandler>.Instance);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(
            new UpdateOwnBrandingCommand(ValidRequest(), "42", "admin"),
            CancellationToken.None));
    }

    private static UpdateOwnBrandingRequest ValidRequest() => new()
    {
        Name = "Brand Seven",
        CompanyName = "Brand Seven S.A.",
        CompanyIdentifier = "ID-7",
        ClientUrl = "https://brand-seven.example",
        SupportEmail = "support@brand-seven.example",
        SupportPhone = "+1 555 0100",
        DocumentType = "invoice",
        LogoUrl = "https://cdn.example/brand-seven.svg",
        PrimaryColor = "#112233",
        SecondaryColor = "#AABBCC",
        BackgroundColor = "#FFFFFF"
    };

    private static List<ValidationResult> Validate(object value)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(value, new ValidationContext(value), results, true);
        return results;
    }

    private sealed class FixedTenantContext(long tenantId) : ITenantContext
    {
        public long TenantId { get; } = tenantId;
    }

    private sealed class RecordingRepository : IBrandConfigurationRepository
    {
        public long? UpdatedBrandId { get; private set; }

        public Task<BrandConfigurationEntity?> GetByBrandIdAsync(long brandId)
            => Task.FromResult<BrandConfigurationEntity?>(null);

        public Task<List<BrandConfigurationEntity>> GetAllAsync()
            => Task.FromResult(new List<BrandConfigurationEntity>());

        public Task<BrandConfigurationEntity> UpsertAsync(BrandConfigurationEntity config)
            => Task.FromResult(config);

        public Task<BrandConfigurationEntity?> UpdateBrandingAsync(
            long brandId,
            BrandConfigurationEntity branding)
        {
            UpdatedBrandId = brandId;
            branding.BrandId = brandId;
            branding.UpdatedAt = DateTime.UtcNow;
            return Task.FromResult<BrandConfigurationEntity?>(branding);
        }

        public Task<BrandConfigurationEntity?> DeleteAsync(long brandId)
            => Task.FromResult<BrandConfigurationEntity?>(null);
    }

    private sealed class RecordingBrandConfigurationProvider : IBrandConfigurationProvider
    {
        public long? InvalidatedBrandId { get; private set; }

        public Task<BrandConfigurationDto?> GetByBrandIdAsync(long brandId)
            => Task.FromResult<BrandConfigurationDto?>(null);

        public Task<IReadOnlyList<BrandConfigurationDto>> GetAllAsync()
            => Task.FromResult<IReadOnlyList<BrandConfigurationDto>>([]);

        public Task InvalidateCacheAsync(long? brandId = null)
        {
            InvalidatedBrandId = brandId;
            return Task.CompletedTask;
        }
    }
}
