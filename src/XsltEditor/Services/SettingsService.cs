using System.IO;
using System.Text.Json;

using XsltEditor.Models;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class SettingsService(string filePath) : ISettingsService
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public Settings? Settings { get; private set; }

    public async Task LoadSettings()
    {
        if (!File.Exists(filePath))
        {
            Settings = new();
            return;
        }

        try
        {
            var data = await File.ReadAllTextAsync(filePath);
            Settings = JsonSerializer.Deserialize<Settings>(data) ?? new();
        }
        catch
        {
            Settings = new();
        }
    }

    public async Task SaveSettings()
    {
        var data = JsonSerializer.Serialize(Settings, _jsonOptions);
        await File.WriteAllTextAsync(filePath, data);
    }
}