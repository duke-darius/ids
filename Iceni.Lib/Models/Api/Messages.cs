namespace Iceni.Lib.Models.Api;

/// <summary>
///     Messages that can be returned by the api
/// </summary>
[Serializable]
public class Messages
{
    /// <summary>
    ///     Rest message returned by the api to signify success
    /// </summary>
    /// <param name="Success"></param>
    /// <param name="Message"></param>
    /// <param name="TranslationId"></param>
    /// <param name="Token"></param>
    /// <param name="AdditionalInfo"></param>
    public record RestMessage(bool Success, string? Message = null, string? TranslationId = null, string? Token = null, string? AdditionalInfo = null);
}