using System.IO;
using System.Runtime.Versioning;
using System.Text.Json;

using XsltEditor.DTO;
using XsltEditor.Mappers;
using XsltEditor.Models;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

[SupportedOSPlatform("windows")]
internal class CompletionDataService : ICompletionDataService
{
    private const string FilePath = "Resources/completions.json";
    private readonly JsonSerializerOptions _options;

    public CompletionDataService()
    {
        _options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
    }

    public IList<CompletionData> LoadCompletionData()
    {
        if (!File.Exists(FilePath))
        {
            return [];
        }

        var json = File.ReadAllText(FilePath);
        var dtoList = JsonSerializer.Deserialize<IList<CompletionDataDto>>(json);

        return CompletionDataMapper.ToModelList(dtoList ?? Enumerable.Empty<CompletionDataDto>()).ToList();
    }

    public async Task SaveCompletionData(IEnumerable<CompletionData> completions)
    {
        var dtoList = CompletionDataMapper.ToDtoList(completions);
        var json = JsonSerializer.Serialize(dtoList, _options);

        if (string.IsNullOrEmpty(json))
        {
            return;
        }

        await File.WriteAllTextAsync(FilePath, json);
    }
}