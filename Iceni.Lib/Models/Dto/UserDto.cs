using Iceni.Lib.EfModels;

namespace Iceni.Lib.Models.Dto;

/// <summary>
///     data transfer object of the AspNetCore user
/// </summary>
[Serializable]
public class UserDto
{
    /// <summary>
    ///     Users Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Users user name
    /// </summary>
    public string? Username { get; set; }
    
    /// <summary>
    ///     Users email
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    ///     Users full name
    /// </summary>
    public string? FullName { get; set; }
    
    /// <summary>
    ///     Users phone number
    /// </summary>
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    ///     Users assigned roles
    /// </summary>
    public IEnumerable<string>? UserRoles { get; set; }


    /// <summary>
    ///     Default ctr
    /// </summary>
    public UserDto(){}

    /// <summary>
    ///     Main ctr
    /// </summary>
    /// <param name="user"></param>
    /// <param name="roles"></param>
    public UserDto(IceniUser user, IEnumerable<string>? roles = null)
    {
        Id = user.Id;
        Username = user.UserName;
        FullName = user.FullName;
        Email = user.Email;

        UserRoles = roles;
    }
}