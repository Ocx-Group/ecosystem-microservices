using Ecosystem.InventoryService.Api.GrpcServices;
using Ecosystem.InventoryService.Api.Middlewares;
using Ecosystem.InventoryService.Data.Context;
using Ecosystem.InventoryService.Infra.IoC;
using Ecosystem.Infra.IoC;
using Ecosystem.Infra.IoC.Extensions;
using Ecosystem.Infra.IoC.MultiTenancy;

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

// Shared infrastructure (RabbitMQ + MassTransit)
var rabbitHost = builder.Configuration["RabbitMQ:Host"] ?? "rabbitmq://localhost";
var rabbitUser = builder.Configuration["RabbitMQ:Username"] ?? "guest";
var rabbitPass = builder.Configuration["RabbitMQ:Password"] ?? "guest";
builder.Services.AddInfrastructure(rabbitHost, rabbitUser, rabbitPass);
builder.Services.AddObjectStorage(builder.Configuration);

// InventoryService dependencies
builder.Services.AddInventoryServiceDependencies(builder.Configuration);

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
app.UseAuthorization();

app.UseTenantResolution();

app.MapHealthChecks("/health");
app.MapGrpcService<InventoryGrpcService>();
app.MapControllers();

await app.ApplyMigrationsAsync<InventoryServiceDbContext>();
await app.RunAsync();
