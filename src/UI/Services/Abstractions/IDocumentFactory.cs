using XsltEditor.Sdk.Enums;
using XsltEditor.ViewModels;

namespace XsltEditor.Services.Abstractions;

public interface IDocumentFactory
{
    DocumentViewModel Create(string name, DocumentType type);
}