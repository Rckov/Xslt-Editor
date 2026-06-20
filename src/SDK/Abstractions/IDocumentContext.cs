namespace XsltEditor.Sdk.Abstractions;

public interface IDocumentContext
{
    IReadOnlyList<IDocument> Documents { get; }
    string? HtmlContent { get; }
}