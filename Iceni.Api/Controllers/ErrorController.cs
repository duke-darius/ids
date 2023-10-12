using Iceni.Lib.Models.Api;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Iceni.Api.Controllers;
/// <summary>
///     Controller for handling unhandled exceptions
/// </summary>
[ApiController]
[Route("[controller]")]
public class ErrorController : Controller
{
    /// <summary>
    ///     In case all goes to pot, this controller catches the exception and wraps it in a way the ApiConsumer can work with
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    [HttpGet]
    [HttpPost]
    [HttpPatch]
    [HttpDelete]
    [HttpPut]
    public IActionResult Index([FromServices] IHostEnvironment host)
    {
        var ctx = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var error = ctx?.Error;
        if (error is not null)
        {
            Log.Error(error, "Failed to complete request");
        }

        if (error is ApiException apiException)
        {
            return BadRequest(new Messages.RestMessage(false, apiException.Code.fallback, apiException.Code.code));
        }

        return BadRequest(new Messages.RestMessage(false, ErrorCodes.UnknownError.fallback, ErrorCodes.UnknownError.code,
            AdditionalInfo: error?.Message));
    }
}