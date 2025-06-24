using Microsoft.Win32;

using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class FileDialogService : IFileDialogService
{
    public string? OpenFileDialog(string title, params string[] extensions)
    {
        var dialog = new OpenFileDialog
        {
            Title = title,
            Filter = BuildFilter(title, extensions)
        };

        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }

    public string? OpenSaveDialog(string title, params string[] extensions)
    {
        var dialog = new SaveFileDialog
        {
            Title = title,
            Filter = BuildFilter(title, extensions)
        };

        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }

    private static string BuildFilter(string title, string[]? extensions)
    {
        if (extensions == null || extensions.Length == 0)
        {
            return "All Files|*.*";
        }

        var filter = string.Join(";", extensions.Select(ext => $"*{ext}"));
        return $"{title} |{filter}";
    }
}