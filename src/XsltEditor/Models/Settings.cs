using System.Runtime.Versioning;

using XsltEditor.Models.Enums;
using XsltEditor.Transform.Enums;

namespace XsltEditor.Models;

[SupportedOSPlatform("windows")]
internal class Settings
{
    public Settings()
    {
        Theme = ThemeType.Dark;
        Engine = EngineType.XslCompiledTransform;
    }

    public ThemeType Theme { get; set; }

    public EngineType Engine { get; set; }
}