using Iceni.Lib.Models.Api;
using Iceni.Lib.Models.Dto;
using Iceni.Lib.Models.Enums;
using RestSharp;

namespace Iceni.Lib.ApiConsumer;

/// <summary>
///     ApiClient for lessons
/// </summary>
public class LessonApiClient : BaseApi
{
    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="client"></param>
    /// <param name="api"></param>
    public LessonApiClient(RestClient client, IceniApiClient api) : base(client, api)
    {
    }

    /// <summary>
    ///     GET /api/pupils
    ///     Queries the available lessons for the current user
    /// </summary>
    /// <returns></returns>
    public async Task<ApiResponse<IEnumerable<LessonDto>>> QueryLessonsForScheduler(DateTime startDate, DateTime endDate)
    {
        var req = NewReq("/api/lesson/scheduler");
        if (req == null)
            return new ApiResponse<IEnumerable<LessonDto>>(null, ErrorCodes.UserNotAuthenticated.ToMessage);

        req.AddQueryParameter("startDate", DateTime.SpecifyKind(startDate, DateTimeKind.Utc).ToString("O"));
        req.AddQueryParameter("endDate", DateTime.SpecifyKind(endDate, DateTimeKind.Utc).ToString("O"));
        
        var res = await Client.ExecuteAsync<IEnumerable<LessonDto>>(req);
        return ValidateResponse(res);
    }

    /// <summary>
    ///     Queries lessons
    /// </summary>
    /// <param name="pupilId"></param>
    /// <param name="lessonType"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    public async Task<ApiResponse<PageOf<LessonDto>>> QueryLessons(Guid? pupilId = null, LessonType? lessonType = null,
        int? skip = null, int? take = null)
    {
        var req = NewReq("/api/lesson");
        if (req == null)
            return new ApiResponse<PageOf<LessonDto>>(null, ErrorCodes.UserNotAuthenticated.ToMessage);

        if (pupilId.HasValue)
            req.AddQueryParameter("pupilId", pupilId.Value);
        if (lessonType.HasValue)
            req.AddQueryParameter("lessonType", lessonType.Value);
        if (skip.HasValue)
            req.AddQueryParameter("skip", skip.Value);
        if (take.HasValue)
            req.AddQueryParameter("take", take.Value);

        var res = await Client.ExecuteAsync<PageOf<LessonDto>>(req);
        return ValidateResponse(res);
    }

    /// <summary>
    ///     Gets a lesson by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ApiResponse<LessonDto>> GetLesson(Guid id)
    {
        var req = NewReq($"/api/lesson/{id}");
        if (req == null)
            return new ApiResponse<LessonDto>(null, ErrorCodes.UserNotAuthenticated.ToMessage);

        var res = await Client.ExecuteAsync<LessonDto>(req);
        return ValidateResponse(res);
    }

    /// <summary>
    ///     Creates a new lesson and returns it
    /// </summary>
    /// <returns></returns>
    public async Task<ApiResponse<LessonDto>> CreateNewLesson(LessonDto newLesson)
    {
        var req = NewReq("/api/lesson", Method.Post);
        if (req == null)
            return new ApiResponse<LessonDto>(null, ErrorCodes.UserNotAuthenticated.ToMessage);

        req.AddJsonBody(newLesson);
        var res = await Client.ExecuteAsync<LessonDto>(req);
        return ValidateResponse(res);
    }

    /// <summary>
    ///     Updates a given lesson and returns the new value
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    public async Task<ApiResponse<LessonDto>> UpdateLesson(LessonDto update)
    {
        var req = NewReq("/api/lesson", Method.Patch);
        if (req == null)
            return new ApiResponse<LessonDto>(null, ErrorCodes.UserNotAuthenticated.ToMessage);

        req.AddJsonBody(update);

        var res = await Client.ExecuteAsync<LessonDto>(req);
        return ValidateResponse(res);
    }

    /// <summary>
    ///     Deletes a given lesson
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Messages.RestMessage> DeleteLesson(Guid id)
    {
        var req = NewReq($"/api/lesson/{id}", Method.Delete);
        if (req == null)
            return ErrorCodes.UserNotAuthenticated.ToMessage;

        var res = await Client.ExecuteAsync(req);
        return ValidateResponse(res);
    }
}