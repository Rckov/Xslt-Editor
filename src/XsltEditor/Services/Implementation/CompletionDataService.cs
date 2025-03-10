using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.Json;

using XsltEditor.Services.Interfaces;
using XsltEditor.Views.UserControls;

namespace XsltEditor.Services.Implementation;

[SupportedOSPlatform("windows")]
internal class CompletionDataService : ICompletionDataService
{
    private const string СompletionResource = "XsltEditor.Resources.completions.json";
    private static readonly string СompletionsPath = Path.Combine(App.SpecialFolder, "completions.json");

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public IList<CompletionData> LoadCompletionData()
    {
        if (!File.Exists(СompletionsPath))
        {
            CopyCompletionsFromResources();
        }

        var json = File.ReadAllText(СompletionsPath);
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

        File.WriteAllText(СompletionsPath, json);
    }

    private void CopyCompletionsFromResources()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(СompletionResource)
            ?? throw new FileNotFoundException("Resource not found: completions.json");

        using var fileStream = File.Create(СompletionsPath);
        stream.CopyTo(fileStream);
    }
}