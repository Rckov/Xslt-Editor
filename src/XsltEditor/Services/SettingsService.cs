using System.Text.Json;

using XsltEditor.Models;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class SettingsService(IFileOperationsService fileService) : ISettingsService
{
    private readonly string _filePath = fileService.GetPath("settings\\settings.json");

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public Settings Settings { get; private set; } = new();

    public async Task LoadSettings()
    {
        try
        {
            if (!fileService.Exists(_filePath))
            {
                await SaveSettings();
                return;
            }

            var json = await fileService.ReadAsync(_filePath);
            var deserializeSettings = JsonSerializer.Deserialize<Settings>(json, _jsonOptions);

            if (deserializeSettings != null)
            {
                Settings = deserializeSettings;
            }
            else
            {
                await SaveSettings();
            }
        }
        catch
        {
            await SaveSettings();
        }
    }

    public async Task SaveSettings()
    {
        if (Settings == null)
        {
            throw new InvalidOperationException("Cannot save settings because Settings is null.");
        }

        var json = JsonSerializer.Serialize(Settings, _jsonOptions);
        await fileService.SaveAsync(_filePath, json);
    }
}