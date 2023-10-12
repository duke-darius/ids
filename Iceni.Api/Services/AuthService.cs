using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Iceni.Lib.EfModels;
using Iceni.Lib.Models.Api;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Iceni.Api.Services;

/// <summary>
///     Service for managing authorization requests
/// </summary>
public class AuthService
{
    private readonly IDbContextFactory<IceniCtx> _dbContextFactory;
    private readonly UserManager<IceniUser> _userManager;
    private readonly RoleManager<IceniRole> _roleManager;
    private readonly IConfiguration _configuration;

    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <param name="userManager"></param>
    /// <param name="roleManager"></param>
    /// <param name="configuration"></param>
    public AuthService(IDbContextFactory<IceniCtx> dbContextFactory, UserManager<IceniUser> userManager, RoleManager<IceniRole> roleManager, IConfiguration configuration)
    {
        _dbContextFactory = dbContextFactory;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }


    /// <summary>
    ///     attempts login via username and password
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <exception cref="ApiException"></exception>
    public async Task<string> AttemptLogin(string username, string password)
    {
        await using var ctx = await _dbContextFactory.CreateDbContextAsync();

        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
            throw new ApiException(ErrorCodes.UnknownError);
        
        var ok = await _userManager.CheckPasswordAsync(user, password);
        if (!ok)
            throw new ApiException(ErrorCodes.UnknownError);

        return await GenerateJwtForUser(user);
    }
    
    /// <summary>
    /// Generates a JWT for a given user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<string> GenerateJwtForUser(IceniUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);

        var roles = _roleManager.Roles.ToDictionary(x => x.Name!, x => x);
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName ?? throw new Exception()),
            new(ClaimTypes.SerialNumber, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Email, user.Email ?? "")
        };
        
        authClaims.AddRange(claims);

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            var roleClaims = await _roleManager.GetClaimsAsync(roles[userRole]);
            foreach (var claim in roleClaims)
            {
                if (authClaims.All(x => x.Type != claim.Type))
                {
                    authClaims.Add(claim);
                }
            }
        }
        
        var token = GetToken(authClaims);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"] ?? throw new InvalidOperationException() ));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.Now.AddDays(30),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}