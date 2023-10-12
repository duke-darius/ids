using Iceni.Lib.Models.Api;
using Iceni.Lib.Models.Dto;
using Iceni.Lib.Models.Enums;
using RestSharp;

namespace Iceni.Lib.ApiConsumer;
/// <summary>
///     Api client for accessing Pupils in the api
/// </summary>
public class PupilApiClient : BaseApi
{
    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="client"></param>
    /// <param name="api"></param>
    public PupilApiClient(RestClient client, IceniApiClient api) : base(client, api)
    {
    }

    /// <summary>
    ///     GET /api/pupils
    ///     Queries the available pupils for the current user
    /// </summary>
    /// <param name="query"></param>
    /// <param name="type"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    public async Task<ApiResponse<PageOf<PupilDto>>> QueryPupils(string? query, PupilType? type, int? skip = null,
        int? take = null)
    {
        var req = NewReq("/api/pupil");
        if (req == null)
            return new ApiResponse<PageOf<PupilDto>>(null, ErrorCodes.UserNotAuthenticated.ToMessage);

        if (!string.IsNullOrEmpty(query))
            req.AddQueryParameter("query", query);
        if (type.HasValue)
            req.AddQueryParameter("type", type.Value);
        if (skip.HasValue)
            req.AddQueryParameter("skip", skip.Value);
        if (take.HasValue)
            req.AddQueryParameter("take", take.Value);

        var res = await Client.ExecuteAsync<PageOf<PupilDto>>(req);
        return ValidateResponse(res);
    }

    /// <summary>
    ///     Gets a pupil by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ApiResponse<PupilDto>> GetPupil(Guid id)
    {
        var req = NewReq($"/api/pupil/{id}");
        if (req == null)
            return new ApiResponse<PupilDto>(null, ErrorCodes.UserNotAuthenticated.ToMessage);

        var res = await Client.ExecuteAsync<PupilDto>(req);
        return ValidateResponse(res);
    }

    /// <summary>
    ///     Creates a new pupil and returns it
    /// </summary>
    /// <returns></returns>
    public async Task<ApiResponse<PupilDto>> CreateNewPupil()
    {
        var req = NewReq("/api/pupil", Method.Post);
        if (req == null)
            return new ApiResponse<PupilDto>(null, ErrorCodes.UserNotAuthenticated.ToMessage);

        var res = await Client.ExecuteAsync<PupilDto>(req);
        return ValidateResponse(res);
    }

    /// <summary>
    ///     Updates a given pupil and returns the new value
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    public async Task<ApiResponse<PupilDto>> UpdatePupil(PupilDto update)
    {
        var req = NewReq("/api/pupil", Method.Patch);
        if (req == null)
            return new ApiResponse<PupilDto>(null, ErrorCodes.UserNotAuthenticated.ToMessage);

        req.AddJsonBody(update);

        var res = await Client.ExecuteAsync<PupilDto>(req);
        return ValidateResponse(res);
    }

    /// <summary>
    ///     Deletes a given pupil
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Messages.RestMessage> DeletePupil(Guid id)
    {
        var req = NewReq($"/api/pupil/{id}", Method.Delete);
        if (req == null)
            return ErrorCodes.UserNotAuthenticated.ToMessage;

        var res = await Client.ExecuteAsync(req);
        return ValidateResponse(res);
    }
}