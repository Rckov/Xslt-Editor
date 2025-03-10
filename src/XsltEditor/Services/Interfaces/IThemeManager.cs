using XsltEditor.Services.Implementation;

namespace XsltEditor.Services.Interfaces;

public interface IThemeManager
{
    ThemeType CurrentTheme { get; }

    void Apply(ThemeType themeType);
}