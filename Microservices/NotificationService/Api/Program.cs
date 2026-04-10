using Asp.Versioning;
using Ecosystem.NotificationService.Api.Middlewares;
using Ecosystem.NotificationService.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

// RabbitMQ + MassTransit with SendEmailConsumer
var rabbitHost = builder.Configuration["RabbitMQ:Host"] ?? "rabbitmq://localhost";
var rabbitUser = builder.Configuration["RabbitMQ:Username"] ?? "guest";
var rabbitPass = builder.Configuration["RabbitMQ:Password"] ?? "guest";

// NotificationService dependencies (MongoDB, MediatR, Brevo, MassTransit, Repositories)
builder.Services.AddNotificationServiceDependencies(builder.Configuration, rabbitHost, rabbitUser, rabbitPass);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();

app.MapHealthChecks("/health");
app.MapControllers();

await app.RunAsync();
