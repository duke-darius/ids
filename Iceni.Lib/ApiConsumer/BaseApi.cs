using Iceni.Lib.Models.Api;
using Newtonsoft.Json;
using RestSharp;

namespace Iceni.Lib.ApiConsumer;

/// <summary>
///     Base Api abstraction for individual api controllers
/// </summary>
public class BaseApi
{
    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="client"></param>
    /// <param name="api"></param>
    protected BaseApi(RestClient client, IceniApiClient api)
    {
        Client = client;
        Api = api;
    }

    /// <summary>
    ///     Rest client
    /// </summary>
    protected RestClient Client { get; }
    
    /// <summary>
    ///     Base client
    /// </summary>
    public IceniApiClient Api { get; }

    /// <summary>
    ///     Creates a new RestRequest and optionally checks if auth is valid
    /// </summary>
    /// <param name="endpoint"></param>
    /// <param name="method"></param>
    /// <param name="requireAuth"></param>
    /// <returns></returns>
    protected RestRequest? NewReq(string endpoint, Method method = Method.Get, bool requireAuth = true)
    {
        var req = new RestRequest(endpoint, method);
        if (requireAuth)
        {
            if (Api.JwtToken == null || Api.Jwt == null || Api.Jwt?.ValidTo <= DateTime.UtcNow.AddMinutes(-10))
            {
                Api.InvokeOnUnauthorisedEvent();
                return null;
            }

            req.AddHeader("Authorization", $"Bearer {Api.JwtToken}");
        }

        return req;
    }

    /// <summary>
    ///     Executes a request
    /// </summary>
    /// <param name="endpoint"></param>
    /// <param name="method"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<ApiResponse<T>> Execute<T>(string endpoint, Method method, Action<RestRequest> action)
    {
        var req = NewReq(endpoint, method);
        if (req == null)
            return new ApiResponse<T>(default, ErrorCodes.UserNotAuthenticated.ToMessage);
        
        action.Invoke(req);

        var res = await Client.ExecuteAsync<T>(req);
        return ValidateResponse(res);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpoint"></param>
    /// <param name="method"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public async Task<Messages.RestMessage> Execute(string endpoint, Method method, Action<RestRequest> action)
    {
        var req = NewReq(endpoint, method);
        if (req == null)
            return ErrorCodes.UserNotAuthenticated.ToMessage;
        
        action.Invoke(req);

        var res = await Client.ExecuteAsync(req);
        return ValidateResponse(res);
    }

    /// <summary>
    ///     Executes a request
    /// </summary>
    /// <param name="endpoint"></param>
    /// <param name="method"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<ApiResponse<T>> Execute<T>(string endpoint, Method method, Func<RestRequest, Task> action)
    {
        var req = NewReq(endpoint, method);
        if (req == null)
            return new ApiResponse<T>(default, ErrorCodes.UserNotAuthenticated.ToMessage);
        
        await action.Invoke(req);

        var res = await Client.ExecuteAsync<T>(req);
        return ValidateResponse(res);
    }

    /// <summary>
    ///     Executes a request
    /// </summary>
    /// <param name="endpoint"></param>
    /// <param name="method"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public async Task<Messages.RestMessage> Execute(string endpoint, Method method, Func<RestRequest, Task> action)
    {
        var req = NewReq(endpoint, method);
        if (req == null)
            return ErrorCodes.UserNotAuthenticated.ToMessage;
        
        await action.Invoke(req);

        var res = await Client.ExecuteAsync(req);
        return ValidateResponse(res);
    }

    /// <summary>
    ///     Validate API response returning either object or error message
    /// </summary>
    /// <param name="res"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected ApiResponse<T> ValidateResponse<T>(RestResponse<T> res)
    {
        if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Api.InvokeOnUnauthorisedEvent();
            return new ApiResponse<T>(default, new Messages.RestMessage(false, ErrorCodes.UserNotAuthenticated.fallback, ErrorCodes.UserNotAuthenticated.code));
        }

        if (res.StatusCode == System.Net.HttpStatusCode.Forbidden)
            return new ApiResponse<T>(default, new Messages.RestMessage(false, ErrorCodes.UserNotAuthenticated.fallback, ErrorCodes.UserNotAuthenticated.code));

        if (!res.IsSuccessful)
        {
            if (TryGetMessage(res.Content, out Messages.RestMessage? msg))
                return new ApiResponse<T>(default, msg,res.Content);
            else
                return new ApiResponse<T>(default, new Messages.RestMessage(false, ErrorCodes.UnknownError.fallback, ErrorCodes.UnknownError.code), res.Content);
        }

        return new ApiResponse<T>(res.Data);
    }

    /// <summary>
    /// Validate API response returning either nothing or error message
    /// </summary>
    /// <param name="res"></param>
    /// <returns></returns>
    public Messages.RestMessage ValidateResponse(RestResponse res)
    {
        if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Api.InvokeOnUnauthorisedEvent();
            return new Messages.RestMessage(false, ErrorCodes.UserNotAuthenticated.fallback, ErrorCodes.UserNotAuthenticated.code);
        }

        if (!res.IsSuccessful)
        {
            if (TryGetMessage(res?.Content, out var msg))
                return msg ?? throw new NullReferenceException("Failed to parse msg");
        }

        return new Messages.RestMessage(true);
    }

    private static bool TryGetMessage(string? content, out Messages.RestMessage? message)
    {
        message = null;
        if (content == null)
            return false;
        try
        {
            var obj = JsonConvert.DeserializeObject<Messages.RestMessage>(content);
            if (obj == null)
                return false;
            message = obj;

            return true;
        }
        catch
        {
            return false;
        }
    }
}