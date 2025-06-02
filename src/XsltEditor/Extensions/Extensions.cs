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
            ".xsl" => DocumentType.Xsl,
            ".xml" => DocumentType.Xml,

            _ => default
        };
    }
}