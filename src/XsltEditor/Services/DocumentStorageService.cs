using System.IO;

using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class DocumentStorageService : IDocumentStorageService
{
    public async Task<string?> ReadContentAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        return await File.ReadAllTextAsync(filePath);
    }

    public async Task WriteContentAsync(string filePath, string? content)
    {
        await File.WriteAllTextAsync(filePath, content);
    }
}