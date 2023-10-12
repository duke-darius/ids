using Iceni.Lib.Models.Api;
using RestSharp;

namespace Iceni.Lib.ApiConsumer;
/// <summary>
///     Client for performing authentication calls
/// </summary>
public class AuthApiClient : BaseApi
{
    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="client"></param>
    /// <param name="api"></param>
    public AuthApiClient(RestClient client, IceniApiClient api) : base(client, api)
    {
    }

    
    /// <summary>
    ///     Attempts to authenticate using a username and password
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<ApiResponse<string>> AuthWithPassword(string username, string password)
    {
        var req = NewReq("/api/auth", Method.Post, false);
        if (req == null)
            return new ApiResponse<string>(null, ErrorCodes.UserNotAuthenticated.ToMessage);
        
        req.AddParameter("username", username);
        req.AddParameter("password", password);

        var res = await Client.ExecuteAsync<string>(req);
        return ValidateResponse(res);
    }
}