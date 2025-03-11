using System.Runtime.Versioning;

using XsltEditor.Services.Implementation;
using XsltEditor.Transform.Enums;

namespace XsltEditor.Models;

[SupportedOSPlatform("windows")]
public class Settings
{
    public Settings()
    {
        Theme = ThemeType.Dark;
        Engine = EngineType.XslCompiledTransform;
        IsRuntimeCompile = true;
    }

    public ThemeType Theme { get; set; }
    public EngineType Engine { get; set; }
    public bool IsRuntimeCompile { get; set; }
}