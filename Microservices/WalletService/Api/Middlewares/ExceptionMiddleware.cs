using Ecosystem.WalletService.Domain.Responses;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace Ecosystem.WalletService.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            logger.LogError(e, "[ExceptionMiddleware] | ERROR: {Message}", e.Message);
            await HandleExceptionAsync(context, e);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new ServicesResponse
        {
            Success = false,
            Code = context.Response.StatusCode,
            Message = exception.Message
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
