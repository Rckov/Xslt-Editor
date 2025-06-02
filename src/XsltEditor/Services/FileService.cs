using Microsoft.Win32;

using System.IO;
using System.Text;

using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class FileService : IFileService
{
    public string? ShowOpenFileDialog(string title, params string[]? allowedExtensions)
    {
        var dialog = new OpenFileDialog
        {
            Title = title,
            Filter = BuildFilter(title, allowedExtensions)
        };

        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }

    public string? ShowSaveFileDialog(string title, params string[]? allowedExtensions)
    {
        var dialog = new SaveFileDialog
        {
            Title = title,
            Filter = BuildFilter(title, allowedExtensions)
        };

        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }

    public async Task<string?> ReadFileContentAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        using var reader = new StreamReader(filePath, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }

    public async Task SaveFileContentAsync(string filePath, string? content)
    {
        ArgumentNullException.ThrowIfNull(filePath);

        using var writer = new StreamWriter(filePath, false, Encoding.UTF8);
        await writer.WriteAsync(content ?? string.Empty);
    }

    public bool IsExists(string filePath) => File.Exists(filePath);

    private string BuildFilter(string title, string[]? extensions)
    {
        if (extensions == null || extensions.Length == 0)
        {
            return "All Files|*.*";
        }

        var filter = string.Join(";", extensions.Select(ext => $"*{ext}"));
        return $"{title} |{filter}";
    }
}