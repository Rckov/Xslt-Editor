using System.IO;
using System.Text;

using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class FileOperationsService : IFileOperationsService
{
    private readonly string _basePath;

    public FileOperationsService()
    {
        _basePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Xslt Editor");

        EnsureDirectory(_basePath);
    }

    public bool Exists(string filePath)
    {
        return File.Exists(filePath);
    }

    public string GetPath(string nameFile)
    {
        return Path.Combine(_basePath, nameFile);
    }

    public async Task<string> ReadAsync(string filePath)
    {
        return await File.ReadAllTextAsync(filePath);
    }

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