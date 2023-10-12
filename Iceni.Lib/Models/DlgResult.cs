using System.Diagnostics.CodeAnalysis;
using Iceni.Lib.Models.Enums;

namespace Iceni.Lib.Models;

/// <summary>
///     Dialog result
/// </summary>
public class DlgResult<T>
{
    /// <summary>
    ///     Result Object
    /// </summary>
    public T Result { get; set; }
    
    /// <summary>
    ///     Action taken
    /// </summary>
    public DialogAction Action { get; set; }

    /// <summary>
    ///     If Action Is not Cancelled and Result is not null
    /// </summary>
    public bool Ok => Action != DialogAction.Cancelled && Result != null;

    /// <summary>
    ///     Ctr
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    public DlgResult(T result, DialogAction action)
    {
        Result = result;
        Action = action;
    }
}