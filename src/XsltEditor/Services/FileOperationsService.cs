using System.IO;
using System.Text;

using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class FileOperationsService : IFileOperationsService
{
    public FileOperationsService()
    {
        BasePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Xslt Editor");
        EnsureDirectory(BasePath);
    }

    public string BasePath { get; }

    public bool Exists(string filePath) 
        => File.Exists(filePath);

    public string GetPath(string nameFile) 
        => Path.Combine(BasePath, nameFile);

    public async Task<string> ReadAsync(string filePath) 
        => await File.ReadAllTextAsync(filePath);

    public async Task SaveAsync(string filePath, string? content)
    {
        var directoryPath = Path.GetDirectoryName(filePath);

        if (!string.IsNullOrEmpty(directoryPath))
        {
            EnsureDirectory(directoryPath);
        }

        await File.WriteAllTextAsync(filePath, content ?? string.Empty, Encoding.UTF8);
    }

    private static void EnsureDirectory(string pathDirectory)
    {
        if (Directory.Exists(pathDirectory))
        {
            return;
        }

        Directory.CreateDirectory(pathDirectory);
    }
}