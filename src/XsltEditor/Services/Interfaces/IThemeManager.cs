using XsltEditor.Services.Implementation;

namespace XsltEditor.Services.Interfaces;

public interface IThemeManager
{
    void Apply(ThemeType themeType);
}