namespace XsltEditor.Services.Interfaces;

internal interface IFileOperationsService
{
    string BasePath { get; }

    string GetPath(string nameFile);

    Task<string> ReadAsync(string filePath);

    Task SaveAsync(string filePath, string? content);

    bool Exists(string filePath);
}