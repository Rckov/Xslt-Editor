using XsltEditor.Models;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class SettingsService : ISettingsService
{
    public Settings LoadSettings()
    {
        return new Settings();
    }

    public void SaveSettings(Settings settings)
    {
    }
}