using ICSharpCode.AvalonEdit.Highlighting;
using XsltEditor.Models;

namespace XsltEditor.Services.Abstractions.Themes;

public interface IThemeService
{
    ThemeType CurrentTheme { get; }
    IHighlightingDefinition? Highlighting { get; }

    void SetTheme(ThemeType theme);

    void SwitchTheme();
}