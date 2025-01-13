using XsltEditor.Models;

namespace XsltEditor.Services.Interfaces;

public interface ISettingsService
{
    void SaveSettings(Settings settings);

    Settings LoadSettings();
}