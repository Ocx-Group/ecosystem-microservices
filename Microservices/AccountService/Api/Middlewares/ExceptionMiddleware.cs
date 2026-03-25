using Ecosystem.AccountService.Api.Models;
using Ecosystem.AccountService.Domain.Exceptions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;

namespace Ecosystem.AccountService.Api.Middlewares;

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

        switch (exception)
        {
            case CustomException customException:
                context.Response.StatusCode = (int)customException.StatusCode;
                return context.Response.WriteAsync(customException.ExceptionBody ?? string.Empty);
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = new ServicesResponse
                {
                    Success = false,
                    Code = context.Response.StatusCode,
                    Message = exception.Message
                };
                return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
