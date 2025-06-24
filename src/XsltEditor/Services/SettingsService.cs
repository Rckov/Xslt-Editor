using System.IO;
using System.Text.Json;

using XsltEditor.Models;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class SettingsService : ISettingsService
{
    private Settings _currentSettings;
    private readonly string SettingsPath = Path.Combine(App.AppDirectory, "settings.json");

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public SettingsService()
    {
        _currentSettings = LoadSettings();
    }

    public Settings Settings => _currentSettings;

    public Settings LoadSettings()
    {
        if (File.Exists(SettingsPath))
        {
            return LoadSettings(SettingsPath);
        }
        else
        {
            return CreateSettings(SettingsPath);
        }
    }

    public void SaveSettings(Settings settings)
    {
        SaveSettings(SettingsPath, settings);
    }

    private Settings LoadSettings(string settingsPath)
    {
        try
        {
            var json = File.ReadAllText(SettingsPath);
            var settings = JsonSerializer.Deserialize<Settings>(json, _jsonOptions);

            return settings ?? new Settings();
        }
        catch
        {
            return CreateSettings(settingsPath);
        }
    }

    private Settings CreateSettings(string settingsPath)
    {
        _currentSettings = new Settings();
        SaveSettings(settingsPath, _currentSettings);

        return _currentSettings;
    }

    private void SaveSettings(string settingsPath, Settings settings)
    {
        var json = JsonSerializer.Serialize(settings, _jsonOptions);
        File.WriteAllText(settingsPath, json);
    }
}