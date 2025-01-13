using System.IO;
using System.Text.Json;

using XsltEditor.Models;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class SettingsService : ISettingsService
{
    private readonly string _settingsPath;

    public SettingsService()
    {
        _settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XsltEditor", "settings.json");
    }

    public Settings LoadSettings()
    {
        Settings settings;

        if (File.Exists(_settingsPath))
        {
            var json = File.ReadAllText(_settingsPath);
            settings = JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
        }
        else
        {
            settings = new Settings();
        }

        return settings;
    }

    public void SaveSettings(Settings settings)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var dir = Path.GetDirectoryName(_settingsPath);
        var json = JsonSerializer.Serialize(settings, options);

        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        File.WriteAllText(_settingsPath, json);
    }
}