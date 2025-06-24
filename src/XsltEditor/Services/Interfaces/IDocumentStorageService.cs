namespace XsltEditor.Services.Interfaces;

internal interface IDocumentStorageService
{
    Task<string?> ReadContentAsync(string filePath);

    Task WriteContentAsync(string filePath, string? content);
}