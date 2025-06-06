using XsltEditor.Models.Enums;

namespace XsltEditor.Services.Interfaces;

internal interface IThemeService
{
    ThemeType CurrentTheme { get; }

    void ChangeTheme(ThemeType themeType);
}