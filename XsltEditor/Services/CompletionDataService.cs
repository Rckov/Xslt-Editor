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
        var list = JsonSerializer.Deserialize<IList<CompletionData>>(json);

        return list ?? [];
    }

    public void SaveCompletionData(IList<CompletionData> completions)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(completions, options);

        if (string.IsNullOrEmpty(json))
        {
            return;
        }

        File.WriteAllText(FilePath, json);
    }
}