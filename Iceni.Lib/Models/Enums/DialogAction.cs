namespace Iceni.Lib.Models.Enums;

/// <summary>
///     Enum for dialog result
/// </summary>
public enum DialogAction
{
    /// <summary>
    ///     Do nothing
    /// </summary>
    /// <returns></returns>
    Cancelled,
    /// <summary>
    ///     Insert it
    /// </summary>
    Insert,
    /// <summary>
    ///     Update it
    /// </summary>
    Update,
    /// <summary>
    ///     Delete it
    /// </summary>
    Delete
}