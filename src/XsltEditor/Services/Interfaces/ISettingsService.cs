using XsltEditor.Models;

namespace XsltEditor.Services.Interfaces;

internal interface ISettingsService
{
    Settings Settings { get; }

    void SaveSettings(Settings settings);
}