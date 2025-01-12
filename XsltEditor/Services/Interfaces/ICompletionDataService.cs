using XsltEditor.Models;

namespace XsltEditor.Services.Interfaces;

public interface ICompletionDataService
{
    IList<CompletionData> LoadCompletionData();

    Task SaveCompletionData(IEnumerable<CompletionData> completions);
}