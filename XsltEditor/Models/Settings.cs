using ICSharpCode.AvalonEdit;

using XsltEditor.Helpers;
using XsltEditor.Models.Base;

namespace XsltEditor.Models;

public class Settings : ObservableObject
{
    public Settings()
    {
        Theme = ThemeType.Dark;
        FontFamily = "Consolas";
        ShowSpaces = false;
        ConvertTabsToSpaces = false;
        HighlightCurrentLine = false;
        TabSize = 4;
        IndentSize = 4;
        RuntimeTransformation = false;
    }

    public ThemeType Theme
    {
        get;
        set => Set(ref field, value);
    }

    public string? FontFamily
    {
        get;
        set => Set(ref field, value);
    }

    public bool ShowSpaces
    {
        get;
        set => Set(ref field, value);
    }

    public bool ConvertTabsToSpaces
    {
        get;
        set => Set(ref field, value);
    }

    public bool HighlightCurrentLine
    {
        get;
        set => Set(ref field, value);
    }

    public int TabSize
    {
        get;
        set => Set(ref field, value);
    }

    public int IndentSize
    {
        get;
        set => Set(ref field, value);
    }

    public bool RuntimeTransformation
    {
        get;
        set => Set(ref field, value);
    }

    public TextEditorOptions TextEditorOptions
    {
        get;
        set => Set(ref field, value);
    }
}