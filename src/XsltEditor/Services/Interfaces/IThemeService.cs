using XsltEditor.Models.Enums;

namespace XsltEditor.Services.Interfaces;

internal interface IThemeService
{
    ThemeType CurrentThemeType { get; }

    void SwitchTheme();

    void SetTheme(ThemeType theme);
}