using System.IO;
using System.Text.Json;

using XsltEditor.Models;
using XsltEditor.Services.Interfaces;
using XsltEditor.Transform.Enums;

namespace XsltEditor.Services;

internal sealed class SettingsService : ISettingsService
{
    private readonly string _filePath;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public SettingsService(string filePath)
    {
        _filePath = filePath;
        Settings = LoadSettings();
    }

    public Settings Settings { get; private set; }

    public Settings LoadSettings()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                Settings = GetDefaultSettings();
                SaveSettings(Settings);
                return Settings;
            }

            var json = File.ReadAllText(_filePath);
            Settings = JsonSerializer.Deserialize<Settings>(json, _jsonOptions) ?? GetDefaultSettings();
        }
        catch
        {
            Settings = GetDefaultSettings();
        }

        return Settings;
    }

    public void SaveSettings()
    {
        SaveSettings(Settings);
    }

    public void SaveSettings(Settings settings)
    {
        Settings = settings;

        var json = JsonSerializer.Serialize(Settings, _jsonOptions);
        File.WriteAllText(_filePath, json);
    }

    public ThemeType GetTheme()
    {
        return Settings.Theme;
    }

    public void SetTheme(ThemeType theme)
    {
        Settings.Theme = theme;
    }

    public EngineType GetEngine()
    {
        return Settings.Engine;
    }

    public void SetEngine(EngineType engine)
    {
        Settings.Engine = engine;
    }

    private static Settings GetDefaultSettings() => new()
    {
        Theme = ThemeType.Dark,
        Engine = EngineType.XslCompiledTransform
    };
}