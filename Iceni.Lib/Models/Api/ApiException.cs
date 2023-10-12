namespace Iceni.Lib.Models.Api;

/// <summary>
///     Defines an exception that can be returned from the Api
/// </summary>
[Serializable]
public class ApiException : Exception
{
    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="code"></param>
    /// <param name="additionalInfo"></param>
    /// <param name="exception"></param>
    public ApiException(ErrorCodes.ErrorCode code, string? additionalInfo = null, Exception? exception = null)
    {
        Code = code;
        AdditionalInfo = additionalInfo;
        Exception = exception;
    }

    /// <summary>
    ///     The ErrorCode
    /// </summary>
    public ErrorCodes.ErrorCode Code { get; }
    
    /// <summary>
    ///     Any addition information the api has provided about the failure (this can be rendered in browser)
    /// </summary>
    public string? AdditionalInfo { get; }
    /// <summary>
    ///     The exception that caused the failure
    /// </summary>
    public Exception? Exception { get; }

    /// <summary>
    ///     Returns the ApiException in a Messages.RestMessage
    /// </summary>
    /// <returns></returns>
    public Messages.RestMessage ToMessage()
    {
        return new Messages.RestMessage(false, Code.fallback, Code.code, AdditionalInfo: AdditionalInfo);
    }
}