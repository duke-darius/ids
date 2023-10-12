namespace Iceni.Lib.Attributes;

/// <summary>
///     Attribute for setting a colour
/// </summary>
public class LessonTextColourAttribute : Attribute
{
    /// <summary>
    ///     Colour
    /// </summary>
    public string Colour { get; }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="colour"></param>
    public LessonTextColourAttribute(string colour)
    {
        Colour = colour;
    }
}