using Microsoft.Win32;

namespace XsltEditor.Helpers;

internal static class Dialog
{
    public static string OpenFile(string title, params string[]? extensions)
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = title,
            Filter = BuildFilter(title, extensions)
        };

        return openFileDialog.ShowDialog().GetValueOrDefault() ? openFileDialog.FileName : string.Empty;
    }

    public static string SaveFile(string title, params string[]? extensions)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Title = title,
            Filter = BuildFilter(title, extensions)
        };

        if (saveFileDialog.ShowDialog().GetValueOrDefault())
        {
            return string.Empty;
        }

        return saveFileDialog.FileName;
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