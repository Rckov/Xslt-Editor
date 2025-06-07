using System.Text.Json;

using XsltEditor.Models;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class CompletionDataService(IFileOperationsService fileService, IResourceOperationsService resourceService) : ICompletionDataService
{
    private readonly List<CompletionData> _data = [];
    private readonly string _filePath = fileService.GetPath("completions.json");

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public IReadOnlyList<CompletionData> Data => _data;

    public async Task LoadCompletionData()
    {
        if (!fileService.Exists(_filePath))
        {
            await RestoreDefaults();
            return;
        }

        var json = await fileService.ReadAsync(_filePath);
        LoadFromJson(json);
    }

    public async Task SaveCompletionData()
    {
        var json = JsonSerializer.Serialize(Data, _jsonOptions);
        await fileService.SaveAsync(_filePath, json);
    }

    public void Add(CompletionData data)
    {
        if (Data.Contains(data))
        {
            return;
        }

        _data.Add(data);
    }

    public void Remove(CompletionData data)
    {
        _data.Remove(data);
    }

    private async Task RestoreDefaults()
    {
        const string defaultResource = "XsltEditor.Resources.completions.json";

        var json = await resourceService.ReadResourceAsStringAsync(defaultResource);
        if (string.IsNullOrWhiteSpace(json))
        {
            return;
        }

        await fileService.SaveAsync(_filePath, json);
        LoadFromJson(json);
    }

    private void LoadFromJson(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return;
        }

        var items = JsonSerializer.Deserialize<List<CompletionData>>(json, _jsonOptions);

        if (items is null)
        {
            return;
        }

        _data.Clear();
        _data.AddRange(items);
    }
}