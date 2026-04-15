using Ecosystem.ConfigurationService.Api.GrpcServices;
using Ecosystem.ConfigurationService.Api.Mappings;
using Ecosystem.ConfigurationService.Data.Context;
using Ecosystem.ConfigurationService.Infra.IoC;
using Ecosystem.Infra.Cache;
using Ecosystem.Infra.IoC;
using Ecosystem.Infra.IoC.Extensions;
using Ecosystem.Infra.IoC.MultiTenancy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

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

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseTenantResolution();
app.MapHealthChecks("/health");
app.MapGrpcService<ConfigurationGrpcService>();
app.MapControllers();

await app.ApplyMigrationsAsync<ConfigurationServiceDbContext>();
await app.RunAsync();
