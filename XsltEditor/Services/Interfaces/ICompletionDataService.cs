using XsltEditor.Views.UserControls;

namespace XsltEditor.Services.Interfaces;

public interface ICompletionDataService
{
    IList<CompletionData> LoadCompletionData();

    void SaveCompletionData(IList<CompletionData> completions);
}