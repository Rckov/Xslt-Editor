using XsltEditor.Models;

namespace XsltEditor.Services.Interfaces;

internal interface ICompletionDataService
{
    IList<CompletionData> Data { get; }

    Task LoadCompletionData();

    Task SaveCompletionData();

    void Add(CompletionData data);

    void Remove(CompletionData data);
}