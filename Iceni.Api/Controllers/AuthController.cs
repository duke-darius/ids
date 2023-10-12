using Iceni.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iceni.Api.Controllers;

/// <summary>
///     Controller for performing authorization request
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : Controller
{
    private readonly AuthService _authService;

    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="authService"></param>
    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    ///     Attempt a login using a username and password
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<string> AttemptLogin([FromForm] string username, [FromForm] string password)
    {
        return await _authService.AttemptLogin(username, password);
    }
}