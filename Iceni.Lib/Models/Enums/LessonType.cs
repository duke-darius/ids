using Iceni.Lib.Attributes;

namespace Iceni.Lib.Models.Enums;

/// <summary>
///     Defines the type of a lesson
/// </summary>
public enum LessonType
{
    /// <summary>
    ///     Standard lesson
    /// </summary>
    [LessonBackgroundColour("#624CAB")]
    [LessonTextColour("#FFFFFF")]
    Lesson,
    /// <summary>
    ///     Available slot
    /// </summary>
    [LessonBackgroundColour("#7189FF")]
    [LessonTextColour("#000000")]
    AvailableSlot,
    /// <summary>
    ///     Tutor is unavailable
    /// </summary>
    [LessonBackgroundColour("#C1CEFE")]
    [LessonTextColour("#000000")]
    UnavailableSlot,
    /// <summary>
    ///     Private appointment with pupil
    /// </summary>
    [LessonBackgroundColour("#758ECD")]
    [LessonTextColour("#000000")]
    PrivateAppointment,
    /// <summary>
    ///     Pupils driving test
    /// </summary>

    [LessonBackgroundColour("#7FD8BE")] 
    [LessonTextColour("#000000")]
    DrivingTest,
    /// <summary>
    ///     Provisional booking with pupil
    /// </summary>
    [LessonBackgroundColour("#A0DDFF")]
    [LessonTextColour("#000000")]
    ProvisionalBooking
}