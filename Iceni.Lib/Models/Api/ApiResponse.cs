namespace Iceni.Lib.Models.Api;

/// <summary>
///     Response from Api
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponse<T>
{
    /// <summary>
    ///     Data of the response if Successful
    /// </summary>
    public T? Data { get; set; }
    /// <summary>
    ///     Error from response if unsuccessful
    /// </summary>
    public Messages.RestMessage? Error { get; set; }

    /// <summary>
    ///     Returns true if the Error is null or Error.success is true
    /// </summary>
    public bool Ok => Error?.Success ?? true;
        
    /// <summary>
    ///     Provides access to the response body if needed
    /// </summary>
    public string? ResponseBody { get; set; }

    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="data"></param>
    /// <param name="error"></param>
    /// <param name="responseBody"></param>
    public ApiResponse(T? data, Messages.RestMessage? error = null, string? responseBody = null)
    {
        Data = data;
        Error = error;
        ResponseBody = responseBody;
    }

    /// <summary>
    ///     Allows matching error codes
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public bool MatchError(ErrorCodes.ErrorCode code) => Error?.TranslationId == code.code;
        
}