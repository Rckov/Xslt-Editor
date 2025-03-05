using System.IO;
using System.Text.Json;

using XsltEditor.Models;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class SettingsService : ISettingsService
{
    private const string SettingsFileName = "settings.json";
    private static readonly string _settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XsltEditor", SettingsFileName);

    public Settings? Settings { get; private set; }

    public Settings LoadSettings()
    {
        try
        {
            if (File.Exists(_settingsPath))
            {
                var json = File.ReadAllText(_settingsPath);
                Settings = JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
            }
            else
            {
                Settings = new Settings();
            }

            return Settings;
        }
        catch
        {
            return new Settings();
        }
    }

    public void SaveSettings()
    {
        var json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_settingsPath, json);
    }
}