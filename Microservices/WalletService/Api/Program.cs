using Ecosystem.WalletService.Infra.IoC;
using Ecosystem.Infra.IoC;
using Ecosystem.Infra.IoC.MultiTenancy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddGrpc();

var rabbitHost = builder.Configuration["RabbitMQ:Host"] ?? "rabbitmq://localhost";
var rabbitUser = builder.Configuration["RabbitMQ:Username"] ?? "guest";
var rabbitPass = builder.Configuration["RabbitMQ:Password"] ?? "guest";
builder.Services.AddInfrastructure(rabbitHost, rabbitUser, rabbitPass);

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
app.UseAuthorization();
app.UseTenantResolution();
app.MapHealthChecks("/health");
app.MapControllers();
app.MapGrpcService<Ecosystem.WalletService.Api.GrpcServices.WalletGrpcService>();

await app.RunAsync();
