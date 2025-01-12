using System.IO;
using System.Runtime.Versioning;
using System.Text.Json;

using XsltEditor.Models;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

[SupportedOSPlatform("windows")]
internal class CompletionDataService : ICompletionDataService
{
    private const string FilePath = "Resources/completions.json";

    public IList<CompletionData> LoadCompletionData()
    {
        if (!File.Exists(FilePath))
        {
            return [];
        }

        var json = File.ReadAllText(FilePath);
        var completionItems = JsonSerializer.Deserialize<IList<CompletionData>>(json);

        return completionItems ?? [];
    }

    public async Task SaveCompletionData(IEnumerable<CompletionData> completions)
    {
        var json = JsonSerializer.Serialize(completions);

        if (string.IsNullOrEmpty(json))
        {
            return;
        }

        await File.WriteAllTextAsync(FilePath, json);
    }
}