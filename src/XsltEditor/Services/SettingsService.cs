using System.IO;
using System.Text.Json;

using XsltEditor.Models;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class SettingsService : ISettingsService
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly string SettingsPath = Path.Combine(App.AppDirectory, "settings.json");

    public SettingsService()
    {
        Settings = LoadSettings();
    }

    public Settings Settings { get; private set; }

    public void SaveSettings(Settings settings)
    {
        SaveSettings(SettingsPath, settings);
    }

    private Settings LoadSettings()
    {
        return File.Exists(SettingsPath)
            ? LoadSettings(SettingsPath)
            : CreateSettings(SettingsPath);
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
        Settings = new Settings();
        SaveSettings(settingsPath, Settings);

        return Settings;
    }

    private void SaveSettings(string settingsPath, Settings settings)
    {
        var json = JsonSerializer.Serialize(settings, _jsonOptions);
        File.WriteAllText(settingsPath, json);
    }
}