using System.Text;
using System.Threading.RateLimiting;
using Ecosystem.GatewayService.Api.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// =============================================================================
// YARP Reverse Proxy
// =============================================================================
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// =============================================================================
// JWT Authentication — validates token structure/signature at the gateway level.
// Downstream services still perform tenant-specific validation.
// =============================================================================
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// =============================================================================
// Rate Limiting
// =============================================================================
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter("global", limiter =>
    {
        limiter.PermitLimit = builder.Configuration.GetValue("RateLimiting:PermitLimit", 100);
        limiter.Window = TimeSpan.FromSeconds(builder.Configuration.GetValue("RateLimiting:WindowSeconds", 10));
        limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiter.QueueLimit = builder.Configuration.GetValue("RateLimiting:QueueLimit", 10);
    });

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            success = false,
            code = StatusCodes.Status429TooManyRequests,
            message = "Too many requests. Please try again later."
        }, cancellationToken);
    };
});

// =============================================================================
// CORS
// =============================================================================
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? ["*"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("gateway-cors", policy =>
    {
        if (allowedOrigins.Contains("*"))
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
        else
        {
            policy.WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        }
    });
});

// =============================================================================
// Health Checks — aggregate downstream service health endpoints
// =============================================================================
var healthChecks = builder.Services.AddHealthChecks();
var downstreamServices = builder.Configuration.GetSection("DownstreamServices").GetChildren();

foreach (var service in downstreamServices)
{
    var url = service["HealthUrl"];
    var name = service.Key;

    if (!string.IsNullOrEmpty(url))
    {
        healthChecks.AddUrlGroup(new Uri(url), name: $"{name}-health", tags: ["downstream"]);
    }
}

// =============================================================================
// Swagger & Controllers
// =============================================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Ecosystem Gateway",
        Version = "v1",
        Description = "API Gateway for Ecosystem Microservices — routes requests to downstream services via YARP."
    });
});

var app = builder.Build();

// =============================================================================
// Middleware Pipeline
// =============================================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway API v1");

        // Aggregate downstream Swagger docs into the Gateway UI
        var services = app.Configuration.GetSection("DownstreamServices").GetChildren();
        foreach (var service in services)
        {
            var swaggerUrl = service["SwaggerUrl"];
            if (!string.IsNullOrEmpty(swaggerUrl))
            {
                options.SwaggerEndpoint(swaggerUrl, $"{service.Key} API");
            }
        }
    });
}

app.UseCors("gateway-cors");
app.UseRateLimiter();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets();
app.MapReverseProxy();

app.MapHealthChecks("/health");
app.MapControllers();

await app.RunAsync();

