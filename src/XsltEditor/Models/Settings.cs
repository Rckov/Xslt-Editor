using System.Runtime.Versioning;

using XsltEditor.Models.Enums;
using XsltEditor.Transform.Enums;

namespace XsltEditor.Models;

[SupportedOSPlatform("windows")]
internal class Settings
{
    public ThemeType Theme { get; set; }

    public EngineType Engine { get; set; }
}