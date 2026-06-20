using System.Collections.ObjectModel;
using XsltEditor.Sdk.Abstractions;
using XsltEditor.Sdk.Enums;
using XsltEditor.ViewModels;

namespace XsltEditor.Services;

public class DocumentContext(ObservableCollection<DocumentViewModel> documents, string? htmlContent) : IDocumentContext
{
    public string? HtmlContent => htmlContent;

    public IReadOnlyList<IDocument> Documents { get; }
        = documents.Select(d => new DocumentAdapter(d)).ToList();

    private sealed class DocumentAdapter(DocumentViewModel vm) : IDocument
    {
        public bool IsReadOnly => vm.IsReadOnly;
        public string Name => vm.Name;
        public DocumentType DocumentType => vm.DocumentType;

        public string? Content
        {
            get => vm.Content;
            set => vm.Content = value;
        }
    }
}