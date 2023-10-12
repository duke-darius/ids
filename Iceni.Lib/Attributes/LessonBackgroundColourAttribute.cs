namespace Iceni.Lib.Attributes;

/// <summary>
///     Attribute for setting a colour
/// </summary>
public class LessonBackgroundColourAttribute : Attribute
{
    /// <summary>
    ///     Colour
    /// </summary>
    public string Colour { get; }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="colour"></param>
    public LessonBackgroundColourAttribute(string colour)
    {
        Colour = colour;
    }
}