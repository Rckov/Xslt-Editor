using System.IO;
using System.Runtime.Versioning;
using System.Text.Json;

using XsltEditor.Models;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services.Implementation;

[SupportedOSPlatform("windows")]
internal class SettingsService : ISettingsService
{
    private const string SettingsFileName = "settings.json";
    private static readonly string SettingsPath = Path.Combine(App.SpecialFolder, SettingsFileName);

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public SettingsService()
    {
        Settings = LoadSettings();
    }

    public Settings Settings { get; }

    public void SaveSettings()
    {
        var directory = Path.GetDirectoryName(SettingsPath) ?? throw new InvalidOperationException("Invalid path settings");

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(Settings, _jsonOptions);
        File.WriteAllText(SettingsPath, json);
    }

    private Settings LoadSettings()
    {
        try
        {
            if (!File.Exists(SettingsPath))
            {
                return new Settings();
            }

            var json = File.ReadAllText(SettingsPath);
            return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
        }
        catch
        {
            return new Settings();
        }
    }
}