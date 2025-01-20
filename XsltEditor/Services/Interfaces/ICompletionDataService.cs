using XsltEditor.Models;

namespace XsltEditor.Services.Interfaces;

internal interface ICompletionDataService
{
    IList<CompletionData> LoadCompletionData();

    void SaveCompletionData(IList<CompletionData> completions);
}