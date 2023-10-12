using Iceni.Api.Services;
using Iceni.Lib.Models.Api;
using Iceni.Lib.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iceni.Api.Controllers;

/// <summary>
///     Controller for managing internal users
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "root")]
public class UserController : Controller
{
    private readonly UserService _userService;

    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="userService"></param>
    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    /// <summary>
    ///     Queries available users
    /// </summary>
    /// <param name="query"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PageOf<UserDto>> QueryUsers(string? query = null, int? skip = null, int? take = null)
    {
        var res = await _userService.QueryUsers(query, skip, take);
        return new PageOf<UserDto>(res.Total, res.Rows.Select(x => new UserDto(x)));
    }

    /// <summary>
    ///     Will create a user, if a password is supplied,
    ///     that password will be used, otherwise we will send the user a "Welcome email"
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<UserDto> CreateUser([FromForm] string username, [FromForm] string? password = null)
    {
        var res = await _userService.CreateUser(username, password);
        return new UserDto(res);
    }

    /// <summary>
    ///     Deletes a given user (VERY DESTRUCTIVE)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        await _userService.DeleteUser(id);
        return Ok();
    }
}