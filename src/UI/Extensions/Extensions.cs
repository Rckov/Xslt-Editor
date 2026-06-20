using System.Collections.ObjectModel;
using XsltEditor.Sdk.Enums;
using XsltEditor.ViewModels;

namespace XsltEditor.Extensions;

public static class Extensions
{
    public static DocumentViewModel? GetDocument(this ObservableCollection<DocumentViewModel> documents,
        DocumentType documentType)
    {
        return documents.FirstOrDefault(x => x.DocumentType == documentType);
    }
}