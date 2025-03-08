using XsltEditor.Services.Implementation;
using XsltEditor.Transform.Enums;

namespace XsltEditor.Models;

public class Settings
{
    public Settings()
    {
        Theme = ThemeType.Dark;
        Engine = EngineType.XslCompiledTransform;
    }

    public ThemeType Theme { get; set; }
    public EngineType Engine { get; set; }
}