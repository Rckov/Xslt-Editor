using System.IO;
using System.Text.Json;

using XsltEditor.Models;
using XsltEditor.Models.Enums;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal sealed class CompletionDataService : ICompletionDataService
{
    private readonly IResourceOperationsService _resourceService;

    private readonly List<CompletionData> _data = [];
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    private string? _currentPath;

    public CompletionDataService(IResourceOperationsService resourceService)
    {
        _resourceService = resourceService;
    }

    public IReadOnlyList<CompletionData> Data
    {
        get
        {
            return _data;
        }
    }

    public void LoadData(DocumentType documentType)
    {
        var fileName = $"completions.{documentType.ToString().ToLowerInvariant()}.json";
        var filePath = Path.Combine(App.AppDirectory, fileName);

        _currentPath = filePath;

        if (!File.Exists(filePath))
        {
            var resourceName = $"XsltEditor.Resources.{fileName}";
            var json = _resourceService.ReadResourceAsStringAsync(resourceName).GetAwaiter().GetResult();

            if (string.IsNullOrWhiteSpace(json))
            {
                return;
            }

            File.WriteAllText(filePath, json);
        }

        var content = File.ReadAllText(filePath);
        var items = JsonSerializer.Deserialize<List<CompletionData>>(content, _jsonOptions);

        if (items is null || items.Count == 0)
        {
            return;
        }

        _data.Clear();
        _data.AddRange(items);
    }

    public void SaveData()
    {
        if (string.IsNullOrWhiteSpace(_currentPath))
        {
            return;
        }

        var json = JsonSerializer.Serialize(_data, _jsonOptions);
        File.WriteAllText(_currentPath, json);
    }

    public void Add(CompletionData data)
    {
        if (_data.Contains(data))
        {
            return;
        }

        _data.Add(data);
    }

    public void Remove(CompletionData data)
    {
        _data.Remove(data);
    }
}