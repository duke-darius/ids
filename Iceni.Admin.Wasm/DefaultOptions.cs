using MudBlazor;

namespace Iceni.Admin.Wasm;

public static class DefaultOptions
{
    public static readonly DialogOptions MediumDialog = new()
    {
        FullWidth = true,
        MaxWidth = MaxWidth.Medium
    };

    public static readonly MudTheme Theme = new()
    {
        Palette = new PaletteLight()
        {
            Primary = "182094",
            PrimaryLighten = "222ED3",
            PrimaryDarken = "171F8C",
            AppbarBackground = "616AE5"
        }
    };
}