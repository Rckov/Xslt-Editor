namespace XsltEditor.Services.Interfaces;

internal interface IFileService
{
    string? ShowOpenFileDialog(string title, params string[]? allowedExtensions);

    string? ShowSaveFileDialog(string title, params string[]? allowedExtensions);

    Task<string?> ReadFileContentAsync(string filePath);

    Task SaveFileContentAsync(string filePath, string? content);

    bool IsExists(string filePath);
}