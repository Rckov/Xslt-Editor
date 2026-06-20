using XsltEditor.Models;

namespace XsltEditor.Services.Abstractions.Themes;

public interface IThemeProvider
{
    ThemeDescriptor Get(ThemeType type);
}