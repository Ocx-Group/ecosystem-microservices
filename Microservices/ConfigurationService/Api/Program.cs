using Ecosystem.ConfigurationService.Api.GrpcServices;
using Ecosystem.ConfigurationService.Api.Mappings;
using Ecosystem.ConfigurationService.Api.Middlewares;
using Ecosystem.ConfigurationService.Data.Context;
using Ecosystem.ConfigurationService.Infra.IoC;
using Ecosystem.Infra.Cache;
using Ecosystem.Infra.IoC;
using Ecosystem.Infra.IoC.Extensions;
using Ecosystem.Infra.IoC.MultiTenancy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = Asp.Versioning.ApiVersionReader.Combine(
        new Asp.Versioning.UrlSegmentApiVersionReader(),
        new Asp.Versioning.HeaderApiVersionReader("x-api-version"));
}).AddMvc().AddApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});
builder.Services.AddGrpc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is not configured.");

if (jwtKey.StartsWith("CHANGE_ME", StringComparison.OrdinalIgnoreCase) ||
    Encoding.UTF8.GetByteCount(jwtKey) < 32)
{
    throw new InvalidOperationException(
        "Jwt:Key must be supplied from a secret and contain at least 32 bytes.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.FromSeconds(30),
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BrandAdministrator", policy => policy
        .RequireAuthenticatedUser()
        .RequireClaim("token_type", "admin")
        .RequireRole("Administrador")
        .RequireAssertion(context =>
        {
            var rawBrandId = context.User.FindFirstValue("brand_id");
            return long.TryParse(rawBrandId, out var brandId) && brandId > 0;
        }));

    // Legacy endpoints expose operational policies beyond visual branding.
    // Dashboard tokens intentionally do not receive this claim.
    options.AddPolicy("BrandConfigurationFullAccess", policy => policy
        .RequireAuthenticatedUser()
        .RequireClaim("token_type", "admin")
        .RequireRole("Administrador")
        .RequireClaim("configuration_scope", "all"));
});

var rabbitHost = builder.Configuration["RabbitMQ:Host"] ?? "rabbitmq://localhost";
var rabbitUser = builder.Configuration["RabbitMQ:Username"] ?? "guest";
var rabbitPass = builder.Configuration["RabbitMQ:Password"] ?? "guest";
builder.Services.AddInfrastructure(rabbitHost, rabbitUser, rabbitPass);

var redisConnection = builder.Configuration.GetConnectionString("RedisConnection") ?? "localhost:6379";
builder.Services.AddSharedCache(redisConnection);

builder.Services.AddConfigurationServiceDependencies(builder.Configuration, typeof(GrpcMappingProfile));

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<AdminTenantContextMiddleware>();
app.UseAuthorization();
TenantResolutionMiddleware.AddSkipPrefix("/configuration.ConfigurationGrpc/");
// Every REST branding endpoint is either explicitly anonymous (public snapshot)
// or protected by the BrandAdministrator JWT policy. The legacy API-client
// token middleware must not reinterpret an admin JWT as a service token.
TenantResolutionMiddleware.AddSkipPrefix("/api/v1/brandconfiguration");
app.UseTenantResolution();
app.MapHealthChecks("/health");
app.MapGet("/health/contracts/brand-configuration-v1", () => Results.Ok(new
{
    status = "ready",
    contract = "brand-configuration-v1"
}));
app.MapGet("/health/contracts/brand-configuration-v2", () => Results.Ok(new
{
    status = "ready",
    contract = "brand-configuration-v2"
}));
app.MapGet("/health/contracts/public-branding-v1", () => Results.Ok(new
{
    status = "ready",
    contract = "public-branding-v1",
    endpoint = "/api/v1/brandconfiguration/public/current",
    telemetryMeter = "Ecosystem.ConfigurationService.Branding"
}));
app.MapGrpcService<ConfigurationGrpcService>();
app.MapControllers();

await app.ApplyMigrationsAsync<ConfigurationServiceDbContext>();
await app.RunAsync();
