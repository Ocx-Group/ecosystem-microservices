using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecosystem.ConfigurationService.Api.Controllers;

public class BaseController : ControllerBase
{
    protected static object Success(object data)
        => new { Success = true, Code = (int)HttpStatusCode.OK, Data = data };

    protected static object Fail(string message)
        => new { Success = false, Code = (int)HttpStatusCode.BadRequest, Message = message };
}
