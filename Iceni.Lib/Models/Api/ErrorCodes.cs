namespace Iceni.Lib.Models.Api;

/// <summary>
///     Strongly typed error codes
/// </summary>
public static class ErrorCodes
{
    /// <summary>
    /// Error Code DTO
    /// </summary>
    public class ErrorCode
    {
        /// <summary>
        ///     Ctr
        /// </summary>
        /// <param name="code"></param>
        /// <param name="fallback"></param>
        public ErrorCode(string code, string fallback)
        {
            this.code = code;
            this.fallback = fallback;
        }

        /// <summary>
        ///     Unique Code
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string code { get; }
            
        /// <summary>
        ///     Human readable message
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string fallback { get; }

        /// <summary>
        ///     Auto prop to convert error code to a new Messages.RestMessage
        /// </summary>
        public Messages.RestMessage ToMessage => new Messages.RestMessage(false, fallback, code);
    }

    /// <summary>
    ///     List of all available ErrorCodes
    /// </summary>
    public static ErrorCode[] AllErrorCodes => typeof(ErrorCodes).GetProperties().Select(x => x.GetValue(null)).Cast<ErrorCode>().ToArray();

    /// <summary>
    ///     Error when trying to access a user that doesn't exist 
    /// </summary>
    public static readonly ErrorCode UserNotFound = new("UserNotFound", "User was not found");

    /// <summary>
    ///     Error when trying to create a duplicate user
    /// </summary>
    public static readonly ErrorCode UserExistsError =
        new("UserExistsError", "A user with this username already exists");


    /// <summary>
    ///     Generic Error, used for anything
    /// </summary>
    public static readonly ErrorCode UnknownError = new("UnknownError", "An unknown error has occurred, it has been reported to our development team");

    /// <summary>
    ///     Used for denying the users request due to an auth issue
    /// </summary>
    public static readonly ErrorCode UserNotAuthenticated = new("UserNotAuthenticated",
        "Current user is not permitted to perform the action. You may need to re-authenticate");
    
    /// <summary>
    ///     Used for denying the users request due to not have the required claims/roles
    /// </summary>
    public static readonly ErrorCode UserNotAuthorized = new("UserNotAuthorized",
        "Current user does not have the correct permissions to perform this action.");
}