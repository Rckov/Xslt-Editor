using XsltEditor.Sdk.Abstractions;
using XsltEditor.ViewModels;

using System.Collections.ObjectModel;
using XsltEditor.Sdk.Enums;

namespace XsltEditor.Services;

internal class DocumentContext(ObservableCollection<DocumentViewModel> documents, string? htmlContent) : IDocumentContext
{
	public string? HtmlContent => htmlContent;
	public IReadOnlyList<IDocument> Documents { get; }
		= documents.Select(d => new DocumentAdapter(d)).ToList();

	private sealed class DocumentAdapter(DocumentViewModel vm) : IDocument
	{
		public string Name => vm.Name;
		public DocumentType DocumentType => vm.DocumentType;
		public string? Content { get => vm.Content; set => vm.Content = value; }
		public bool IsReadOnly => vm.IsReadOnly;
	}
}
