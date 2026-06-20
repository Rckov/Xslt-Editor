using System.IO;
using System.Text.Json;
using XsltEditor.Models;
using XsltEditor.Services.Abstractions;

namespace XsltEditor.Services;

public class SettingsService : ISettingsService
{
    private static readonly string FilePath =
        Path.Combine(AppContext.BaseDirectory, "XsltEditor.settings.json");

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public Settings Settings { get; } = Load();

    public void Save()
    {
        var json = JsonSerializer.Serialize(Settings, JsonOptions);
        File.WriteAllText(FilePath, json);
    }

    private static Settings Load()
    {
        if (!File.Exists(FilePath))
        {
            return new Settings();
        }

        try
        {
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
        }
        catch
        {
            return new Settings();
        }
    }
}