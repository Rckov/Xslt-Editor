using System.IO;
using System.Runtime.Versioning;
using System.Text.Json;

using XsltEditor.Services.Interfaces;
using XsltEditor.Views.UserControls;

namespace XsltEditor.Services.Implementation;

[SupportedOSPlatform("windows")]
internal class CompletionDataService : ICompletionDataService
{
    private const string FilePath = "Resources/completions.json";

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

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
        var json = JsonSerializer.Serialize(completions, _jsonOptions);

        if (string.IsNullOrEmpty(json))
        {
            return;
        }

        File.WriteAllText(FilePath, json);
    }
}