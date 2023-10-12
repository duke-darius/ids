using Iceni.Api.Services;
using Iceni.Lib.EfModels;
using Iceni.Lib.Models.Api;
using Iceni.Lib.Models.Dto;
using Iceni.Lib.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iceni.Api.Controllers;

/// <summary>
///     Controller enabling users to manage lessons
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "adm")]
public class LessonController : Controller
{
    private readonly LessonService _lessonService;

    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="lessonService"></param>
    public LessonController(LessonService lessonService)
    {
        _lessonService = lessonService;
    }

    /// <summary>
    ///     Queries lessons for display in the scheduler
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    [HttpGet("scheduler")]
    public async Task<IEnumerable<LessonDto>> QueryLessonForScheduler(DateTime startDate, DateTime endDate)
    {
        var res = await _lessonService.QueryLessonForScheduler(startDate, endDate);
        return res.Select(x => new LessonDto(x));
    }

    /// <summary>
    ///     Queries lessons 
    /// </summary>
    /// <param name="pupilId"></param>
    /// <param name="lessonType"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PageOf<LessonDto>> QueryLessons(Guid? pupilId, LessonType? lessonType = null, int? skip = null,
        int? take = null)
    {
        var res = await _lessonService.QueryLessons(pupilId, lessonType, skip, take);
        return new PageOf<LessonDto>(res.Total, res.Rows.Select(x=> new LessonDto(x)));
    }

    /// <summary>
    ///     Gets lesson by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<LessonDto> GetLesson(Guid id)
    {
        var res = await _lessonService.GetLesson(id);
        return new LessonDto(res);
    }

    /// <summary>
    ///     Creates a new lesson
    /// </summary>
    /// <param name="newLesson"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<LessonDto> CreateLesson([FromBody] LessonDto newLesson)
    {
        return new LessonDto(await _lessonService.CreateLesson(newLesson));
    }

    /// <summary>
    ///     Updates a lesson
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    [HttpPatch]
    public async Task<LessonDto> UpdateLesson([FromBody] LessonDto update)
    {
        return new LessonDto(await _lessonService.UpdateLesson(update));
    }

    /// <summary>
    ///     Deletes a lesson by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteLesson(Guid id)
    {
        await _lessonService.DeleteLesson(id);
        return Ok();
    }
}