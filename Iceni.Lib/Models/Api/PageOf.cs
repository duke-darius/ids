namespace Iceni.Lib.Models.Api;

/// <summary>
///     Page of T
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class PageOf<T>
{
    /// <summary>
    ///     Default ctr
    /// </summary>
    public PageOf()
    {

    }

    /// <summary>
    ///     Main ctr
    /// </summary>
    /// <param name="total"></param>
    /// <param name="rows"></param>
    public PageOf(int total, IEnumerable<T> rows)
    {
        Total = total;
        Rows = rows;
    }

    /// <summary>
    ///     Rows of T
    /// </summary>
    public IEnumerable<T> Rows { get; set; } = Enumerable.Empty<T>();
    /// <summary>
    ///     Total rows available
    /// </summary>
    public int Total { get; set; }
}