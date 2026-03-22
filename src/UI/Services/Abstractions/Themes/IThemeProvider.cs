using XsltEditor.Models;

namespace XsltEditor.Services.Abstractions.Themes;

internal interface IThemeProvider
{
	ThemeDescriptor Get(ThemeType type);
}