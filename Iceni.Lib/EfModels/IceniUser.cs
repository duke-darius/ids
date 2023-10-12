using Microsoft.AspNetCore.Identity;

namespace Iceni.Lib.EfModels;

/// <summary>
///     Main User class
/// </summary>
public class IceniUser : IdentityUser<Guid>
{
    /// <summary>
    ///     Full name of user
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    ///     List of pupils assigned to a user (Tutor)
    /// </summary>
    public ICollection<Pupil> Pupils { get; set; } = new List<Pupil>();
    
    /// <summary>
    ///     List of lessons assigned to the tutor
    /// </summary>
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}