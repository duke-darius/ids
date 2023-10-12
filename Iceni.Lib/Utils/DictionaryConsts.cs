namespace Iceni.Lib.Utils;

/// <summary>
///     Contains some constants
/// </summary>
public static class DictionaryConsts
{
    
    /// <summary>
    ///     Disables autocomplete in MudTextField
    /// </summary>
    public static readonly Dictionary<string, object> DisableAutocomplete =
        new() 
        {
            { "autocomplete", "off" },
        };
}