using Iceni.Lib.EfModels;
using Iceni.Lib.Models.Api;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Iceni.Api.Services;

/// <summary>
///     Service for managing internal users
/// </summary>
public class UserService
{
    private readonly UserManager<IceniUser> _userManager;
    private readonly IDbContextFactory<IceniCtx> _contextFactory;

    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="contextFactory"></param>
    public UserService(UserManager<IceniUser> userManager, IDbContextFactory<IceniCtx> contextFactory)
    {
        _userManager = userManager;
        _contextFactory = contextFactory;
    }

    /// <summary>
    ///     Queries available users
    /// </summary>
    /// <param name="query"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    public async Task<PageOf<IceniUser>> QueryUsers(string? query, int? skip, int? take)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();

        var users = ctx.Users.AsQueryable();

        if (!string.IsNullOrEmpty(query))
        {
            users = users.Where(x =>
                (x.FullName ?? "").Contains(query) || (x.UserName ?? string.Empty).Contains(query));
        }

        var count = await users.CountAsync();
        if (skip.HasValue)
            users = users.Skip(skip.Value);
        if (take.HasValue)
            users = users.Take(take.Value);

        return new PageOf<IceniUser>(count, await users.ToListAsync());
    }

    /// <summary>
    ///     Creates a new user and automatically assigns it a 'adm' role
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <exception cref="ApiException"></exception>
    public async Task<IceniUser> CreateUser(string username, string? password = null)
    {
        var exists = await _userManager.FindByNameAsync(username);
        if (exists != null)
            throw new ApiException(ErrorCodes.UserExistsError);

        var userCreateRes = await _userManager.CreateAsync(new IceniUser()
        {
            UserName = username,
            Email = username
        });

        if (!userCreateRes.Succeeded)
        {
            throw new ApiException(ErrorCodes.UnknownError,
                string.Join(", ", userCreateRes.Errors.Select(x => x.Description)));
        }

        var user = await _userManager.FindByNameAsync(username);
        if(user == null)
            throw new ApiException(ErrorCodes.UnknownError);
        if (!string.IsNullOrEmpty(password))
        {
            await _userManager.AddPasswordAsync(user, password);
        }
        else
        {
            // Send email
        }

        _ = await _userManager.AddToRoleAsync(user, "adm");

        return user;
    }

    /// <summary>
    ///     Deletes a user with a given Id (THIS WILL DELETE ALL DATA ASSIGNED TO THE USER) 
    /// </summary>
    /// <param name="id"></param>
    public async Task DeleteUser(Guid id)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        var exists = await ctx.Users.FirstOrDefaultAsync(x => x.Id == id);
        
        if (exists == null)
            throw new ApiException(ErrorCodes.UserNotFound);
        
        if (exists.UserName == "root")
            throw new ApiException(ErrorCodes.UnknownError, "CANNOT DELETE ROOT USER");

        ctx.Users.Remove(exists);
        await ctx.SaveChangesAsync();
    }
}