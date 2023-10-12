using System.Security.Claims;
using Iceni.Lib.EfModels;
using Microsoft.EntityFrameworkCore;

namespace Iceni.Api.Services;

/// <summary>
///     Service for managing a current user
/// </summary>
public class CurrentUserService
{
    private readonly IDbContextFactory<IceniCtx> _contextFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="contextFactory"></param>
    /// <param name="httpContextAccessor"></param>
    public CurrentUserService(IDbContextFactory<IceniCtx> contextFactory, IHttpContextAccessor httpContextAccessor)
    {
        _contextFactory = contextFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    ///     Returns the current user
    /// </summary>
    /// <returns></returns>
    public async Task<IceniUser> GetCurrentUser()
    {
        var userId = GetCurrentUsersId();

        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Users.SingleAsync(x => x.Id == userId);
    }

    /// <summary>
    ///     Returns the current users Id
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    /// <exception cref="Exception"></exception>
    public Guid GetCurrentUsersId()
    {
        var http = _httpContextAccessor.HttpContext ??
                   throw new NullReferenceException("HttpContext was not accessible");

        var userId = http.User.FindFirst(ClaimTypes.SerialNumber)?.Value ??
                     throw new NullReferenceException("User Id not accessible");

        if (!Guid.TryParse(userId, out var userIdGuid))
            throw new Exception("Failed to parse user id");
        return userIdGuid;
    }
}