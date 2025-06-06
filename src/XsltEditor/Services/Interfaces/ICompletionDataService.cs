using XsltEditor.Models;

namespace XsltEditor.Services.Interfaces;

internal interface ICompletionDataService
{
    IList<CompletionData> Data { get; }
    IList<CompletionData> LoadCompletionData();

    void SaveCompletionData(IList<CompletionData> completions);

    void Add(CompletionData data);
    void Remove(CompletionData data);
}