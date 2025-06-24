using XsltEditor.Models;

namespace XsltEditor.Services.Interfaces;

internal interface ISettingsService
{
    Settings LoadSettings();

    void SaveSettings(Settings settings);
}