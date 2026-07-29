using System.Text.Json;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Services;
using Ecosystem.ConfigurationService.Domain.Models;
using Ecosystem.Grpc.Configuration;
using Xunit;

namespace Ecosystem.ConfigurationService.ContractTests;

public sealed class PublicBrandingContractTests
{
    [Theory]
    [InlineData("example.com", "example.com")]
    [InlineData("WWW.Example.com.", "example.com")]
    [InlineData("https://www.example.com/path", "example.com")]
    [InlineData("münich.example", "xn--mnich-kva.example")]
    public void NormalizeHost_ProducesStableTenantKey(string input, string expected)
    {
        Assert.Equal(expected, PublicBrandingResolver.NormalizeHost(input));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("https://")]
    public void Resolve_RejectsInvalidHostWithoutChoosingTenant(string input)
    {
        var result = PublicBrandingResolver.Resolve(input, [CreateConfiguration(1, "example.com")]);

        Assert.Equal(PublicBrandingResolutionStatus.InvalidHost, result.Status);
        Assert.Null(result.Branding);
    }

    [Fact]
    public void Resolve_ReturnsOnlyTheActiveConfigurationForHost()
    {
        var active = CreateConfiguration(7, "https://www.example.com");
        var inactive = CreateConfiguration(8, "example.com");
        inactive.IsActive = false;

        var result = PublicBrandingResolver.Resolve("example.com", [inactive, active]);

        Assert.Equal(PublicBrandingResolutionStatus.Resolved, result.Status);
        Assert.Equal(7, result.Branding!.BrandId);
        Assert.Equal("tenant-7", result.Branding.ClientId);
        Assert.Equal("Brand 7", result.Branding.Name);
    }

    [Fact]
    public void Resolve_FailsClosedWhenHostIsAmbiguous()
    {
        var result = PublicBrandingResolver.Resolve(
            "example.com",
            [
                CreateConfiguration(1, "example.com"),
                CreateConfiguration(2, "https://www.example.com")
            ]);

        Assert.Equal(PublicBrandingResolutionStatus.Ambiguous, result.Status);
        Assert.Null(result.Branding);
        Assert.Equal(2, result.MatchCount);
    }

    [Fact]
    public void PublicDto_ContainsOnlyTheApprovedBootstrapFields()
    {
        var actualProperties = typeof(PublicBrandingDto)
            .GetProperties()
            .Select(property => property.Name)
            .Order()
            .ToArray();
        var approvedProperties = new[]
        {
            "BackgroundColor",
            "BrandId",
            "ClientId",
            "ClientUrl",
            "CompanyName",
            "DocumentType",
            "LogoUrl",
            "Name",
            "PrimaryColor",
            "SecondaryColor",
            "SupportEmail",
            "SupportPhone"
        }.Order().ToArray();

        Assert.Equal(approvedProperties, actualProperties);
    }

    [Fact]
    public void PublicDto_SerializesWithTheWebContractCasingAndNoInternalFields()
    {
        var resolution = PublicBrandingResolver.Resolve(
            "example.com",
            [CreateConfiguration(4, "example.com")]);
        var json = JsonSerializer.Serialize(
            resolution.Branding,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));

        Assert.Contains("\"brandId\":4", json);
        Assert.Contains("\"clientId\":\"tenant-4\"", json);
        Assert.DoesNotContain("adminUserName", json, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("senderEmail", json, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("commission", json, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("withdrawal", json, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("blockchain", json, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void InternalGrpcBrandContract_PreservesFieldNamesAndNumbers()
    {
        var actual = BrandConfigurationMessage.Descriptor.Fields
            .InFieldNumberOrder()
            .Select(field => $"{field.FieldNumber}:{field.Name}")
            .ToArray();
        var expected = new[]
        {
            "1:brand_id",
            "2:name",
            "3:sender_name",
            "4:sender_email",
            "5:client_url",
            "6:company_name",
            "7:support_email",
            "8:company_identifier",
            "9:support_phone",
            "10:document_type",
            "11:logo_url",
            "12:primary_color",
            "13:secondary_color",
            "14:background_color",
            "15:is_active",
            "16:admin_user_name",
            "17:email_template_folder",
            "18:commission_enabled",
            "19:commission_levels",
            "20:bonus_percentage",
            "21:pdf_template_name",
            "22:default_father_affiliate_id",
            "23:activate_on_registration",
            "24:default_payment_group_id",
            "25:trading_academy_payment_group_id",
            "26:withdrawal_validation_type",
            "27:withdrawal_time_zone",
            "28:withdrawal_start_hour",
            "29:withdrawal_end_hour",
            "30:withdrawal_cap_no_directs",
            "31:requires_10_percent_purchase_rule",
            "32:pool_validation_required",
            "33:con_payment_enabled",
            "34:con_payment_address",
            "35:blockchain_network_id"
        };

        Assert.Equal(expected, actual);
    }

    private static BrandConfiguration CreateConfiguration(long brandId, string clientUrl)
        => new()
        {
            Id = brandId,
            BrandId = brandId,
            AdminUserName = "not-public",
            SenderName = "not-public",
            SenderEmail = "not-public@example.com",
            EmailTemplateFolder = "not-public",
            ClientUrl = clientUrl,
            PdfTemplateName = "invoice",
            CompanyName = $"Company {brandId}",
            SupportEmail = $"support-{brandId}@example.com",
            LogoUrl = $"https://cdn.example.com/{brandId}.svg",
            PrimaryColor = "#112233",
            SecondaryColor = "#445566",
            BackgroundColor = "#ffffff",
            IsActive = true,
            Brand = new Brand
            {
                Id = brandId,
                Name = $"Brand {brandId}",
                SecretKey = $"tenant-{brandId}",
                IsActive = true
            }
        };
}
