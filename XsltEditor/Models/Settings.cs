using XsltEditor.Helpers;

namespace XsltEditor.Models;

internal class Settings
{
    public Settings()
    {
        Theme = ThemeType.Dark;
        FontSize = 12;
        FontFamily = "Consolas";
        ShowLineNumbers = true;
        ConvertTabsToSpaces = true;
        EnableEmailHyperlinks = false;
        EnableHyperlinks = false;
        EnableCodeFolding = true;
        TabSize = 4;
        IndentSize = 4;
        RuntimeTransformation = false;
    }

    public ThemeType Theme { get; set; }

    public int FontSize { get; set; }

    public string FontFamily { get; set; }

    public bool ShowLineNumbers { get; set; }

    public bool ConvertTabsToSpaces { get; set; }

    public bool EnableEmailHyperlinks { get; set; }

    public bool EnableHyperlinks { get; set; }

    public bool EnableCodeFolding { get; set; }

    public int TabSize { get; set; }

    public int IndentSize { get; set; }

    public bool RuntimeTransformation { get; set; }
}