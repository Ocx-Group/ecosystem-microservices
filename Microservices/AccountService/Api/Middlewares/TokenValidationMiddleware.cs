using Ecosystem.AccountService.Api.Models;
using Ecosystem.AccountService.Domain.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using System.Net;

namespace Ecosystem.AccountService.Api.Middlewares;

public class TokenValidationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context,
        IApiClientRepository apiClientService,
        IBrandRepository brandRepository,
        ILogger<TokenValidationMiddleware> logger)
    {
        if (context.Request.Path == "/health" || context.Request.Path.StartsWithSegments("/ticketHub"))
        {
            await next(context);
            return;
        }

        var token = context.Request.Headers["Authorization"].ToString();
        var secretKey = context.Request.Headers["X-Client-ID"].ToString();

        if (string.IsNullOrEmpty(token) && context.Request.HasFormContentType)
        {
            var form = await context.Request.ReadFormAsync();
            token = form["Authorization"]!;
        }

        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(secretKey))
        {
            await RespondUnauthorized(context, logger, "Token o ClientID no encontrados");
            return;
        }

        var isValidToken = await apiClientService.ValidateApiClient(token);
        var brand = await brandRepository.GetBrandBySecretKeyAsync(secretKey);

        if (!isValidToken || brand == null)
        {
            await RespondUnauthorized(context, logger, "Token o ClientID inválidos");
            return;
        }

        context.Items["brandId"] = brand.Id;

        logger.LogInformation("Solicitud aceptada. URL: {Url}, Marca: {Brand}",
            context.Request.GetDisplayUrl(), brand.Name);

        await next(context);
    }

    private static async Task RespondUnauthorized(HttpContext context, ILogger logger, string reason)
    {
        var response = new ServicesResponse
        {
            Success = false,
            Code = (int)HttpStatusCode.Unauthorized,
            Message = "No autorizado"
        };

        logger.LogInformation("No autorizado, {Reason}. URL: {Url}",
            reason, context.Request.GetDisplayUrl());

        context.Response.StatusCode = 401;
        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
