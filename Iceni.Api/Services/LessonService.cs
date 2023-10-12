using System.Data.Common;
using Iceni.Lib.EfModels;
using Iceni.Lib.Models.Api;
using Iceni.Lib.Models.Dto;
using Iceni.Lib.Models.Enums;
using Iceni.Lib.Utils;
using Microsoft.EntityFrameworkCore;

namespace Iceni.Api.Services;

/// <summary>
///     Service for managing lessons
/// </summary>
public class LessonService
{
    private readonly IDbContextFactory<IceniCtx> _contextFactory;
    private readonly CurrentUserService _currentUserService;

    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="contextFactory"></param>
    /// <param name="currentUserService"></param>
    public LessonService(IDbContextFactory<IceniCtx> contextFactory, CurrentUserService currentUserService)
    {
        _contextFactory = contextFactory;
        _currentUserService = currentUserService;
    }

    /// <summary>
    ///     Queries available lessons for display in the scheduler
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Lesson>> QueryLessonForScheduler(DateTime startDate, DateTime endDate)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();

        return await ctx.Lessons.Where(x => x.TutorId == _currentUserService.GetCurrentUsersId() &&
                                            x.Start >= startDate && x.End <= endDate).ToListAsync();
    }

    /// <summary>
    ///     Queries lessons
    /// </summary>
    /// <param name="pupilId"></param>
    /// <param name="lessonType"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    public async Task<PageOf<Lesson>> QueryLessons(Guid? pupilId = null, LessonType? lessonType = null, int? skip = null, int? take = null)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();

        var lessons = ctx.Lessons.AsQueryable();

        if (pupilId.HasValue)
            lessons = lessons.Where(x => x.PupilId == pupilId.Value);
        if (lessonType.HasValue)
            lessons = lessons.Where(x => x.LessonType == lessonType.Value);

        var count = await lessons.CountAsync();
        if (skip.HasValue)
            lessons = lessons.Skip(skip.Value);
        if (take.HasValue)
            lessons = lessons.Take(take.Value);

        var res = await lessons.ToListAsync();
        return new PageOf<Lesson>(count, res);
    }

    /// <summary>
    ///     Gets a lesson by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includePayment"></param>
    /// <returns></returns>
    public async Task<Lesson> GetLesson(Guid id, bool includePayment = false)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        if (includePayment)
            return await ctx.Lessons.SingleAsync(x => x.Id == id);
        
        return await ctx.Lessons.SingleAsync(x => x.Id == id);
    }

    /// <summary>
    ///     Creates a new lesson
    /// </summary>
    /// <param name="newLesson"></param>
    /// <returns></returns>
    public async Task<Lesson> CreateLesson(LessonDto newLesson)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();

        var lesson = new Lesson()
        {
            TutorId = _currentUserService.GetCurrentUsersId(),
            Start = newLesson.Start,
            End = newLesson.End,
            LessonType = newLesson.LessonType,
            LessonTitle = newLesson.LessonTitle,
            PupilId = newLesson.PupilId,
        };

        ctx.Lessons.Add(lesson);

        await ctx.SaveChangesAsync();
        return lesson;
    }

    /// <summary>
    ///     Updates a lesson
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    public async Task<Lesson> UpdateLesson(LessonDto update)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();

        var lesson = await GetLesson(update.Id, true);
        
        lesson.Start = update.Start;
        lesson.End = update.End;
        lesson.LessonTitle = update.LessonTitle;
        lesson.PupilId = update.PupilId;
        lesson.LessonType = update.LessonType;

        ctx.Update(lesson);
        await ctx.SaveChangesAsync();
        return lesson;
    }

    /// <summary>
    ///     Deletes a lesson by id
    /// </summary>
    /// <param name="id"></param>
    public async Task DeleteLesson(Guid id)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        
        var lesson = await GetLesson(id);
        ctx.Lessons.Remove(lesson);
        
        await ctx.SaveChangesAsync();
    }
}