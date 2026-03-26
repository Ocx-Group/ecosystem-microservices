using System.Net;
using Ecosystem.InventoryService.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.InventoryService.Api.Controllers;

public class BaseController : ControllerBase
{
    protected static ServicesResponse Success(object data)
        => new() { Success = true, Code = (int)HttpStatusCode.OK, Data = data };

    protected static ServicesResponse Fail(string message)
        => new() { Success = false, Code = (int)HttpStatusCode.BadRequest, Message = message };
}
