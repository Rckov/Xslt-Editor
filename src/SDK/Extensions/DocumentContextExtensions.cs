using XsltEditor.Sdk.Abstractions;
using XsltEditor.Sdk.Enums;

namespace XsltEditor.Sdk.Extensions;

public static class DocumentContextExtensions
{
    extension(IDocumentContext context)
    {
        public IDocument? GetDocument(DocumentType type)
        {
            return context.Documents.FirstOrDefault(d => d.DocumentType == type);
        }
    }
}