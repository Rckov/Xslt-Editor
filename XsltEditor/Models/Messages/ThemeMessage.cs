using XsltEditor.Services.Implementation;

namespace XsltEditor.Models.Messages;

internal record ThemeMessage(ThemeType ThemeType)
{
    public bool IsDark
    {
        get => ThemeType == ThemeType.Dark;
    }
}