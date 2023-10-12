using Iceni.Lib.Models.Api;
using Iceni.Lib.Models.Dto;
using RestSharp;

namespace Iceni.Lib.ApiConsumer;

/// <summary>
///     Allows consumption of the Users controller
/// </summary>
public class UserApiClient : BaseApi
{
    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="client"></param>
    /// <param name="api"></param>
    public UserApiClient(RestClient client, IceniApiClient api) : base(client, api)
    {
    }

    /// <summary>
    ///     Queries available users
    /// </summary>
    /// <param name="query"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    public async Task<ApiResponse<PageOf<UserDto>>> QueryUsers(string? query = null, int? skip = null, int? take = null)
    {
        var req = NewReq("/api/user");
        if (req == null)
            return new ApiResponse<PageOf<UserDto>>(null, ErrorCodes.UserNotAuthenticated.ToMessage);

        if (!string.IsNullOrEmpty(query))
            req.AddQueryParameter("query", query);
        if (skip.HasValue)
            req.AddQueryParameter("skip", skip.Value);
        if (take.HasValue)
            req.AddQueryParameter("take", take.Value);

        var res = await Client.ExecuteAsync<PageOf<UserDto>>(req);
        return ValidateResponse(res);
    }

    /// <summary>
    ///     Creates a new user and returns it
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<ApiResponse<UserDto>> CreateUser(string username, string? password = null)
    {
        var req = NewReq("/api/user", Method.Post);
        if (req == null)
            return new ApiResponse<UserDto>(null, ErrorCodes.UserNotAuthenticated.ToMessage);

        req.AddParameter("username", username);
        if (!string.IsNullOrEmpty(password))
            req.AddParameter("password", password);

        var res = await Client.ExecuteAsync<UserDto>(req);
        return ValidateResponse(res);
    }

    /// <summary>
    ///     Deletes a user with a given Id, This will delete all data assigned to a user
    ///     USE WITH CAUTION
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Messages.RestMessage> DeleteUser(Guid id)
    {
        var req = NewReq($"/api/user/{id}", Method.Delete);
        if (req == null)
            return ErrorCodes.UserNotAuthenticated.ToMessage;

        var res = await Client.ExecuteAsync(req);
        return ValidateResponse(res);
    }
}