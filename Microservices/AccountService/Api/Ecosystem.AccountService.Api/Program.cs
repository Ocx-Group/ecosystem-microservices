using Ecosystem.AccountService.Api.Middlewares;
using Ecosystem.AccountService.Infra.IoC;
using Ecosystem.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);

// Core services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();

// Shared infrastructure (RabbitMQ + MassTransit)
var rabbitHost = builder.Configuration["RabbitMQ:Host"] ?? "rabbitmq://localhost";
var rabbitUser = builder.Configuration["RabbitMQ:Username"] ?? "guest";
var rabbitPass = builder.Configuration["RabbitMQ:Password"] ?? "guest";
builder.Services.AddInfrastructure(rabbitHost, rabbitUser, rabbitPass);

// AccountService dependencies (MediatR, Repositories, Services)
builder.Services.AddAccountServiceDependencies();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<TokenValidationMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.MapHealthChecks("/health");
app.MapControllers();

await app.RunAsync();