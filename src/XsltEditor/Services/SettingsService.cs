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

    private Settings _settings;

    public SettingsService(string filePath)
    {
        _filePath = filePath;
        _settings = LoadSettings();
    }

    public Settings Settings => _settings;

    public Settings LoadSettings()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                _settings = GetDefaultSettings();
                SaveSettings(_settings);
                return _settings;
            }

            var json = File.ReadAllText(_filePath);
            _settings = JsonSerializer.Deserialize<Settings>(json, _jsonOptions) ?? GetDefaultSettings();
        }
        catch
        {
            _settings = GetDefaultSettings();
        }

        return _settings;
    }

    public void SaveSettings()
    {
        SaveSettings(_settings);
    }

    public void SaveSettings(Settings settings)
    {
        _settings = settings;

        var json = JsonSerializer.Serialize(_settings, _jsonOptions);
        File.WriteAllText(_filePath, json);
    }

    public ThemeType GetTheme()
    {
        return _settings.Theme;
    }

    public void SetTheme(ThemeType theme)
    {
        _settings.Theme = theme;
    }

    public EngineType GetEngine()
    {
        return _settings.Engine;
    }

    public void SetEngine(EngineType engine)
    {
        _settings.Engine = engine;
    }

    private static Settings GetDefaultSettings() => new()
    {
        Theme = ThemeType.Dark,
        Engine = EngineType.XslCompiledTransform
    };
}