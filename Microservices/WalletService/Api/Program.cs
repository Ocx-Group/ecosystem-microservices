using Ecosystem.Infra.Cache;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Infra.IoC;
using Ecosystem.Infra.IoC;
using Ecosystem.Infra.IoC.Extensions;
using Ecosystem.Infra.IoC.MultiTenancy;
using Ecosystem.WalletService.Api.Middlewares;
using Ecosystem.WalletService.Domain.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var httpPort = int.Parse(builder.Configuration["ASPNETCORE_HTTP_PORTS"] ?? "8080");
var grpcPort = int.Parse(builder.Configuration["ASPNETCORE_GRPC_PORT"] ?? "50051");

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(httpPort, listenOptions =>
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2);
    options.ListenAnyIP(grpcPort, listenOptions =>
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2);
});

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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddGrpc();

// Until now every wallet endpoint was reached with the legacy static service token
// and no [Authorize] at all. The monthly liquidation credits real balances, so it is
// gated behind the same admin JWT that ConfigurationService already validates for the
// dashboard — same key, same issuer, same audience, so one admin login covers both.
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
});

var rabbitHost = builder.Configuration["RabbitMQ:Host"] ?? "rabbitmq://localhost";
var rabbitUser = builder.Configuration["RabbitMQ:Username"] ?? "guest";
var rabbitPass = builder.Configuration["RabbitMQ:Password"] ?? "guest";
builder.Services.AddInfrastructure(rabbitHost, rabbitUser, rabbitPass);

var redisConnection = builder.Configuration.GetConnectionString("RedisConnection") ?? "localhost:6379";
builder.Services.AddSharedCache(redisConnection);

builder.Services.AddWalletServiceDependencies(builder.Configuration);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<IpnBufferingMiddleware>();
app.UseAuthentication();
app.UseMiddleware<AdminTenantContextMiddleware>();
app.UseAuthorization();
TenantResolutionMiddleware.AddSkipPrefix("/wallet.WalletGrpc/");
// The monthly liquidation authenticates with an admin JWT and takes its brand from
// the brand_id claim. Without this skip the legacy X-Client-ID middleware would try
// to reinterpret that bearer token as a service token and reject the request.
TenantResolutionMiddleware.AddSkipPrefix(MonthlyCommissionConstants.RoutePrefix);
// CoinPay posts notifications without an X-Client-ID header; the brand is resolved
// from the stored transaction instead.
TenantResolutionMiddleware.AddSkipPrefix(CoinPayConstants.WebhookPath);
// Same for CoinPayments. Beyond the missing header, tenant resolution reads the form when no
// Authorization is present, which would consume the IPN body before the controller sees it.
TenantResolutionMiddleware.AddSkipPrefix(CoinPaymentsConstants.IpnPath);
TenantResolutionMiddleware.AddSkipPrefix(CoinPaymentsConstants.MatrixIpnPath);
app.UseTenantResolution();
app.MapHealthChecks("/health");
app.MapControllers();
app.MapGrpcService<Ecosystem.WalletService.Api.GrpcServices.WalletGrpcService>();

await app.ApplyMigrationsAsync<WalletServiceDbContext>();
await app.RunAsync();
