using XsltEditor.Sdk.Enums;

namespace XsltEditor.Sdk.Abstractions;

public interface IDocument
{
    string Name { get; }
    string? Content { get; set; }
    DocumentType DocumentType { get; }
}