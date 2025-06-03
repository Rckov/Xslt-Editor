using System.Collections.ObjectModel;
using System.IO;

using XsltEditor.ViewModels;

namespace XsltEditor.Extensions;

internal static class Extensions
{
    public static DocumentType GetDocumentType(this string filePath)
    {
        var extension = Path.GetExtension(filePath)?.ToLowerInvariant();

        return extension switch
        {
            ".xsl" => DocumentType.XSL,
            ".xml" => DocumentType.XML,

            _ => default
        };
    }

    public static DocumentViewModel? GetDocument(this ObservableCollection<DocumentViewModel> documents, DocumentType documentType)
    {
        return documents.FirstOrDefault(x => x.DocumentType == documentType);
    }

    public static void LoadFromEnum<T>(this ICollection<T> collection) where T : struct, Enum
    {
        foreach (var item in Enum.GetValues<T>())
        {
            collection.Add(item);
        }
    }
}