using XsltEditor.Models;

namespace XsltEditor.Services.Interfaces;

public interface ISettingsService
{
    Settings Settings { get; }

    Settings LoadSettings();

    void SaveSettings();
}