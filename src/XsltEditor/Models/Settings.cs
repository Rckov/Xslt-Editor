using XsltEditor.Models.Enums;
using XsltEditor.Transform.Enums;

namespace XsltEditor.Models;

internal class Settings
{
    public ThemeType Theme { get; set; }

    public EngineType EngineType { get; set; }
}