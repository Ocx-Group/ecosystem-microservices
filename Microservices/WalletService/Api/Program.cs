using Ecosystem.Infra.Cache;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Infra.IoC;
using Ecosystem.Infra.IoC;
using Ecosystem.Infra.IoC.Extensions;
using Ecosystem.Infra.IoC.MultiTenancy;
using Ecosystem.WalletService.Api.Middlewares;
using Ecosystem.WalletService.Domain.Constants;

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
app.UseAuthorization();
TenantResolutionMiddleware.AddSkipPrefix("/wallet.WalletGrpc/");
// CoinPay posts notifications without an X-Client-ID header; the brand is resolved
// from the stored transaction instead.
TenantResolutionMiddleware.AddSkipPrefix(CoinPayConstants.WebhookPath);
app.UseTenantResolution();
app.MapHealthChecks("/health");
app.MapControllers();
app.MapGrpcService<Ecosystem.WalletService.Api.GrpcServices.WalletGrpcService>();

await app.ApplyMigrationsAsync<WalletServiceDbContext>();
await app.RunAsync();
