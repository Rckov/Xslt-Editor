using System.Text.Json;

using XsltEditor.Models;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class CompletionDataService : ICompletionDataService
{
    private readonly string _filePath;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public CompletionDataService(string filePath)
    {
        _filePath = filePath;
        Data = LoadCompletionData();
    }

    public IList<CompletionData> Data { get; }
    
    public IList<CompletionData> LoadCompletionData()
    {
        throw new NotImplementedException();
    }

    public void SaveCompletionData(IList<CompletionData> completions)
    {
        throw new NotImplementedException();
    }

    public void Add(CompletionData data)
    {
        throw new NotImplementedException();
    }

    public void Remove(CompletionData data)
    {
        throw new NotImplementedException();
    }
}