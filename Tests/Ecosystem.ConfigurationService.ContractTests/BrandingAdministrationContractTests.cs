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
using BrandEntity = Ecosystem.ConfigurationService.Domain.Models.Brand;

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
    [InlineData(nameof(BrandConfigurationController.GetOwnMonthlyCommissionSettings))]
    [InlineData(nameof(BrandConfigurationController.UpdateOwnMonthlyCommissionSettings))]
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

        Assert.Equal(UpdateOwnBrandingStatus.Updated, result.Status);
        Assert.Equal(7, repository.UpdatedBrandId);
        Assert.Equal(7, result.Branding!.BrandId);
        Assert.Equal(7, provider.InvalidatedBrandId);
    }

    [Fact]
    public async Task UpdateHandler_RejectsHostAlreadyOwnedByAnotherActiveBrand()
    {
        var repository = new RecordingRepository
        {
            Existing = { ActiveConfiguration(9, "https://www.brand-seven.example/portal") }
        };
        var provider = new RecordingBrandConfigurationProvider();
        var handler = new UpdateOwnBrandingHandler(
            repository,
            provider,
            new FixedTenantContext(7),
            NullLogger<UpdateOwnBrandingHandler>.Instance);

        var result = await handler.Handle(
            new UpdateOwnBrandingCommand(ValidRequest(), "42", "admin"),
            CancellationToken.None);

        Assert.Equal(UpdateOwnBrandingStatus.HostConflict, result.Status);
        Assert.Null(result.Branding);
        Assert.Null(repository.UpdatedBrandId);
        Assert.Null(provider.InvalidatedBrandId);
    }

    [Fact]
    public async Task UpdateHandler_AllowsKeepingTheHostOwnedByTheSameBrand()
    {
        var repository = new RecordingRepository
        {
            Existing = { ActiveConfiguration(7, "https://brand-seven.example") }
        };
        var handler = new UpdateOwnBrandingHandler(
            repository,
            new RecordingBrandConfigurationProvider(),
            new FixedTenantContext(7),
            NullLogger<UpdateOwnBrandingHandler>.Instance);

        var result = await handler.Handle(
            new UpdateOwnBrandingCommand(ValidRequest(), "42", "admin"),
            CancellationToken.None);

        Assert.Equal(UpdateOwnBrandingStatus.Updated, result.Status);
        Assert.Equal(7, repository.UpdatedBrandId);
    }

    [Theory]
    [InlineData(false, null)]
    [InlineData(true, "deleted")]
    [InlineData(true, "brand-inactive")]
    public async Task UpdateHandler_IgnoresHostsOfConfigurationsThatCannotResolve(
        bool isActive,
        string? disabledBy)
    {
        var other = ActiveConfiguration(9, "https://brand-seven.example");
        other.IsActive = isActive;
        if (disabledBy == "deleted") other.DeletedAt = DateTime.UtcNow;
        if (disabledBy == "brand-inactive") other.Brand.IsActive = false;

        var repository = new RecordingRepository { Existing = { other } };
        var handler = new UpdateOwnBrandingHandler(
            repository,
            new RecordingBrandConfigurationProvider(),
            new FixedTenantContext(7),
            NullLogger<UpdateOwnBrandingHandler>.Instance);

        var result = await handler.Handle(
            new UpdateOwnBrandingCommand(ValidRequest(), "42", "admin"),
            CancellationToken.None);

        Assert.Equal(UpdateOwnBrandingStatus.Updated, result.Status);
    }

    [Fact]
    public async Task UpdateHandler_RejectsClientUrlWithoutResolvableHost()
    {
        var repository = new RecordingRepository();
        var provider = new RecordingBrandConfigurationProvider();
        var handler = new UpdateOwnBrandingHandler(
            repository,
            provider,
            new FixedTenantContext(7),
            NullLogger<UpdateOwnBrandingHandler>.Instance);

        var result = await handler.Handle(
            new UpdateOwnBrandingCommand(
                ValidRequest() with { ClientUrl = "   " },
                "42",
                "admin"),
            CancellationToken.None);

        Assert.Equal(UpdateOwnBrandingStatus.InvalidHost, result.Status);
        Assert.Null(repository.UpdatedBrandId);
        Assert.Null(provider.InvalidatedBrandId);
    }

    [Fact]
    public async Task UpdateHandler_ReportsNotFoundWhenTheTenantHasNoConfiguration()
    {
        var handler = new UpdateOwnBrandingHandler(
            new RecordingRepository { UpdateReturnsNull = true },
            new RecordingBrandConfigurationProvider(),
            new FixedTenantContext(7),
            NullLogger<UpdateOwnBrandingHandler>.Instance);

        var result = await handler.Handle(
            new UpdateOwnBrandingCommand(ValidRequest(), "42", "admin"),
            CancellationToken.None);

        Assert.Equal(UpdateOwnBrandingStatus.NotFound, result.Status);
        Assert.Null(result.Branding);
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

    [Fact]
    public void MonthlyCommissionRequest_DoesNotAllowClientSelectedBrandId()
    {
        Assert.Null(typeof(UpdateMonthlyCommissionSettingsRequest).GetProperty("BrandId"));
    }

    [Fact]
    public async Task MonthlyCommissionHandler_UsesAuthenticatedTenantAndInvalidatesItsCache()
    {
        var repository = new RecordingRepository();
        var provider = new RecordingBrandConfigurationProvider();

        var result = await MonthlyCommissionHandler(repository, provider, tenantId: 7).Handle(
            MonthlyCommissionCommand(ValidMonthlyCommissionRequest()),
            CancellationToken.None);

        Assert.Equal(UpdateMonthlyCommissionSettingsStatus.Updated, result.Status);
        Assert.Equal(7, repository.UpdatedBrandId);
        Assert.Equal(7, result.Settings!.BrandId);
        Assert.Equal(7, provider.InvalidatedBrandId);
    }

    /// <summary>
    /// The rate reaches the database as a NUMERIC(5,2). Storing it rounded is what keeps
    /// the configured, displayed and actually paid percentages the same number.
    /// </summary>
    [Fact]
    public async Task MonthlyCommissionHandler_RoundsTheRateToTheStoredScale()
    {
        var repository = new RecordingRepository();

        var result = await MonthlyCommissionHandler(repository).Handle(
            MonthlyCommissionCommand(
                ValidMonthlyCommissionRequest() with { InterestRate = 4.005m }),
            CancellationToken.None);

        Assert.Equal(UpdateMonthlyCommissionSettingsStatus.Updated, result.Status);
        Assert.Equal(4.01m, result.Settings!.InterestRate);
    }

    [Theory]
    [InlineData(true, 4, 2, null)]      // enabled without a payment group
    [InlineData(true, 0, 2, 11)]        // enabled with nothing to pay
    [InlineData(false, -1, 2, 11)]      // negative rate
    [InlineData(false, 101, 2, 11)]     // rate over the cap
    [InlineData(false, 4, -1, 11)]      // negative waiting days
    [InlineData(false, 4, 91, 11)]      // waiting days over the cap
    [InlineData(false, 4, 2, 0)]        // non-positive payment group
    public async Task MonthlyCommissionHandler_RejectsSettingsThatWouldBreakThePayout(
        bool enabled, decimal interestRate, int waitingDays, int? paymentGroupId)
    {
        var repository = new RecordingRepository();
        var provider = new RecordingBrandConfigurationProvider();

        var result = await MonthlyCommissionHandler(repository, provider).Handle(
            MonthlyCommissionCommand(new UpdateMonthlyCommissionSettingsRequest
            {
                Enabled = enabled,
                InterestRate = interestRate,
                WaitingDays = waitingDays,
                PaymentGroupId = paymentGroupId
            }),
            CancellationToken.None);

        Assert.Equal(UpdateMonthlyCommissionSettingsStatus.InvalidSettings, result.Status);
        Assert.NotNull(result.ValidationMessage);
        Assert.Null(result.Settings);
        Assert.Null(repository.UpdatedBrandId);
        Assert.Null(provider.InvalidatedBrandId);
    }

    /// <summary>
    /// RecyBot liquidates on the invoice total, and that procedure takes no payment group.
    /// Demanding one anyway would make the only brand that needs it impossible to enable.
    /// </summary>
    [Fact]
    public async Task MonthlyCommissionHandler_AllowsAnEnabledInvoiceTotalBrandWithoutAPaymentGroup()
    {
        var repository = new RecordingRepository
        {
            StoredMonthlyCommissionSource = MonthlyCommissionSources.InvoiceTotal
        };

        var result = await MonthlyCommissionHandler(repository).Handle(
            MonthlyCommissionCommand(
                ValidMonthlyCommissionRequest() with { PaymentGroupId = null }),
            CancellationToken.None);

        Assert.Equal(UpdateMonthlyCommissionSettingsStatus.Updated, result.Status);
        Assert.Equal(MonthlyCommissionSources.InvoiceTotal, result.Settings!.Source);
    }

    /// <summary>
    /// The three dashboards do not send this field. Treating its absence as a reset would
    /// switch RecyBot back to a procedure that finds none of its invoices, silently, the
    /// next time an administrator saved the rate.
    /// </summary>
    [Fact]
    public async Task MonthlyCommissionHandler_KeepsTheStoredSourceWhenTheRequestOmitsIt()
    {
        var repository = new RecordingRepository
        {
            StoredMonthlyCommissionSource = MonthlyCommissionSources.InvoiceTotal
        };

        var result = await MonthlyCommissionHandler(repository).Handle(
            MonthlyCommissionCommand(ValidMonthlyCommissionRequest()),
            CancellationToken.None);

        Assert.Equal(UpdateMonthlyCommissionSettingsStatus.Updated, result.Status);
        Assert.Equal(
            MonthlyCommissionSources.InvoiceTotal,
            repository.UpdatedMonthlyCommissionSource);
    }

    [Fact]
    public async Task MonthlyCommissionHandler_RejectsAnUnknownSource()
    {
        var repository = new RecordingRepository();

        var result = await MonthlyCommissionHandler(repository).Handle(
            MonthlyCommissionCommand(
                ValidMonthlyCommissionRequest() with { Source = "WholeLedger" }),
            CancellationToken.None);

        Assert.Equal(UpdateMonthlyCommissionSettingsStatus.InvalidSettings, result.Status);
        Assert.Null(repository.UpdatedBrandId);
    }

    [Fact]
    public async Task MonthlyCommissionHandler_FailsClosedWithoutAuthenticatedTenant()
    {
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            MonthlyCommissionHandler(tenantId: 0).Handle(
                MonthlyCommissionCommand(ValidMonthlyCommissionRequest()),
                CancellationToken.None));
    }

    [Fact]
    public async Task MonthlyCommissionHandler_ReportsNotFoundWhenTheTenantHasNoConfiguration()
    {
        var result = await MonthlyCommissionHandler(
                new RecordingRepository { UpdateReturnsNull = true }).Handle(
            MonthlyCommissionCommand(ValidMonthlyCommissionRequest()),
            CancellationToken.None);

        Assert.Equal(UpdateMonthlyCommissionSettingsStatus.NotFound, result.Status);
        Assert.Null(result.Settings);
    }

    private static UpdateOwnMonthlyCommissionSettingsHandler MonthlyCommissionHandler(
        RecordingRepository? repository = null,
        RecordingBrandConfigurationProvider? provider = null,
        long tenantId = 7)
        => new(
            repository ?? new RecordingRepository(),
            provider ?? new RecordingBrandConfigurationProvider(),
            new FixedTenantContext(tenantId),
            NullLogger<UpdateOwnMonthlyCommissionSettingsHandler>.Instance);

    private static UpdateOwnMonthlyCommissionSettingsCommand MonthlyCommissionCommand(
        UpdateMonthlyCommissionSettingsRequest request)
        => new(request, "42", "admin");

    private static UpdateMonthlyCommissionSettingsRequest ValidMonthlyCommissionRequest() => new()
    {
        Enabled = true,
        InterestRate = 4m,
        WaitingDays = 2,
        PaymentGroupId = 11
    };

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

    private static BrandConfigurationEntity ActiveConfiguration(long brandId, string clientUrl) => new()
    {
        BrandId = brandId,
        ClientUrl = clientUrl,
        IsActive = true,
        Brand = new BrandEntity { Id = brandId, Name = $"Brand {brandId}", IsActive = true }
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
        public string? UpdatedMonthlyCommissionSource { get; private set; }
        public List<BrandConfigurationEntity> Existing { get; } = [];
        public bool UpdateReturnsNull { get; init; }

        /// <summary>
        /// The stored source this repository reports. The monthly commission handler reads
        /// the brand before validating, because an omitted source on the request means
        /// "keep what is stored".
        /// </summary>
        public string StoredMonthlyCommissionSource { get; init; } = "PaymentGroup";

        public Task<BrandConfigurationEntity?> GetByBrandIdAsync(long brandId)
            => Task.FromResult(UpdateReturnsNull
                ? null
                : new BrandConfigurationEntity
                {
                    BrandId = brandId,
                    MonthlyCommissionSource = StoredMonthlyCommissionSource
                });

        public Task<List<BrandConfigurationEntity>> GetAllAsync()
            => Task.FromResult(Existing);

        public Task<BrandConfigurationEntity> UpsertAsync(BrandConfigurationEntity config)
            => Task.FromResult(config);

        public Task<BrandConfigurationEntity?> UpdateBrandingAsync(
            long brandId,
            BrandConfigurationEntity branding)
        {
            if (UpdateReturnsNull)
                return Task.FromResult<BrandConfigurationEntity?>(null);

            UpdatedBrandId = brandId;
            branding.BrandId = brandId;
            branding.UpdatedAt = DateTime.UtcNow;
            return Task.FromResult<BrandConfigurationEntity?>(branding);
        }

        public Task<BrandConfigurationEntity?> UpdateCommissionSettingsAsync(
            long brandId,
            bool commissionEnabled,
            decimal[] commissionLevels,
            bool dailyBonusAlwaysDistribute)
            => Task.FromResult<BrandConfigurationEntity?>(null);

        public Task<BrandConfigurationEntity?> UpdateMonthlyCommissionSettingsAsync(
            long brandId,
            bool enabled,
            decimal interestRate,
            int waitingDays,
            int? paymentGroupId,
            string source)
        {
            if (UpdateReturnsNull)
                return Task.FromResult<BrandConfigurationEntity?>(null);

            UpdatedBrandId = brandId;
            UpdatedMonthlyCommissionSource = source;
            return Task.FromResult<BrandConfigurationEntity?>(new BrandConfigurationEntity
            {
                BrandId = brandId,
                MonthlyCommissionEnabled = enabled,
                MonthlyCommissionInterestRate = interestRate,
                MonthlyCommissionWaitingDays = waitingDays,
                MonthlyCommissionPaymentGroupId = paymentGroupId,
                MonthlyCommissionSource = source,
                UpdatedAt = DateTime.UtcNow
            });
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
