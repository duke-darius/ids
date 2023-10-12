using Iceni.Lib.Models.Enums;

namespace Iceni.Lib.EfModels;

/// <summary>
///     Defines a lesson
/// </summary>
public class Lesson
{
    /// <summary>
    ///     Lessons unique Id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    ///     Type of lesson
    /// </summary>
    public LessonType LessonType { get; set; }
    /// <summary>
    ///     Start time of the lesson
    /// </summary>
    public DateTime Start { get; set; }
    
    /// <summary>
    ///     End time of the lesson
    /// </summary>
    public DateTime End { get; set; }

    /// <summary>
    ///     AUTO: Duration of the lesson
    /// </summary>
    public TimeSpan Duration => End - Start;

    /// <summary>
    ///     Optional title of the lesson
    /// </summary>
    public string? LessonTitle { get; set; }

    /// <summary>
    ///     Tutor running the lesson
    /// </summary>
    public virtual IceniUser Tutor { get; set; } = null!;
    
    /// <summary>
    ///     Tutor running the lesson
    /// </summary>
    public Guid TutorId { get; set; }
    
    /// <summary>
    ///     Pupil assigned to the lesson
    /// </summary>
    public virtual Pupil? Pupil { get; set; }
    
    /// <summary>
    ///     Pupil assigned to the lesson
    /// </summary>
    public Guid? PupilId { get; set; }



}