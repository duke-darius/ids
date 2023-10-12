using System.ComponentModel;
using System.Reflection;
using Iceni.Lib.Attributes;
using Iceni.Lib.Models.Enums;

namespace Iceni.Lib.Utils;

/// <summary>
///     Provides some helper methods for enums
/// </summary>
public static class EnumExtensions
{

    /// <summary>
    ///     returns a readable string for the LessonType enum
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static string ToReadableString(this LessonType t)
    {
        return t switch
        {
            LessonType.Lesson => "Lesson",
            LessonType.AvailableSlot => "Available slot",
            LessonType.UnavailableSlot => "Unavailable slot",
            LessonType.PrivateAppointment => "Private appointment",
            LessonType.DrivingTest => "Driving test",
            LessonType.ProvisionalBooking => "Provisional booking",
            _=> "UNKNOWN TYPE!!!"
        };
    }
    
    /// <summary>
    ///     Converts a enum to a readable string using the [Description] attribute
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string? ToDescription(this Enum value)
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        if (name is null)
        {
            return null;
        }
            
        var field = type.GetField(name);
        if (field is null)
        {
            return null;
        }
            
        var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
        return attr?.Description;
    }
    
    /// <summary>
    ///     Gets the background colour for an enum
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns> 
    public static string ToBackgroundColour(this LessonType value)
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        if (name is null)
        {
            return "#000000";
        }
            
        var field = type.GetField(name);
        if (field is null)
        {
            return "#000000";
        }
            
        var attr = Attribute.GetCustomAttribute(field, typeof(LessonBackgroundColourAttribute)) as LessonBackgroundColourAttribute;
        return attr?.Colour ?? "#000000";
    }
    
    /// <summary>
    ///     Gets the text colour for a lesson type
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToTextColour(this LessonType value)
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        if (name is null)
        {
            return "#000000";
        }
            
        var field = type.GetField(name);
        if (field is null)
        {
            return "#000000";
        }
            
        var attr = Attribute.GetCustomAttribute(field, typeof(LessonTextColourAttribute)) as LessonTextColourAttribute;
        return attr?.Colour ?? "#000000";
    }
}