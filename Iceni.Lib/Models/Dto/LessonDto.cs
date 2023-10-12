using Iceni.Lib.EfModels;
using Iceni.Lib.Models.Enums;

namespace Iceni.Lib.Models.Dto;

/// <summary>
///     Lesson Data Transfer Object
/// </summary>
[Serializable]
public class LessonDto
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
    public Guid TutorId { get; set; }
    
    /// <summary>
    ///     Pupil assigned to the lesson
    /// </summary>
    public PupilDto? Pupil { get; set; }
    
    /// <summary>
    ///     Pupil assigned to the lesson
    /// </summary>
    public Guid? PupilId { get; set; }

    /// <summary>
    ///     default ctr
    /// </summary>
    public LessonDto(){}

    /// <summary>
    ///     main ctr
    /// </summary>
    /// <param name="lesson"></param>
    public LessonDto(Lesson lesson)
    {
        Id = lesson.Id;
        LessonType = lesson.LessonType;
        Start = lesson.Start;
        End = lesson.End;
        LessonTitle = lesson.LessonTitle;
        TutorId = lesson.TutorId;
        PupilId = lesson.PupilId;
        
        if(lesson.Pupil != null)
            Pupil = new PupilDto(lesson.Pupil);
    }
}