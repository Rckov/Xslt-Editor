using XsltEditor.Models;
using XsltEditor.Transform.Enums;

namespace XsltEditor.Services.Interfaces;

internal interface ISettingsService
{
    Settings Settings { get; }

    Settings LoadSettings();

    void SaveSettings();

    void SaveSettings(Settings settings);

    ThemeType GetTheme();

    void SetTheme(ThemeType theme);

    EngineType GetEngine();

    void SetEngine(EngineType engine);
}