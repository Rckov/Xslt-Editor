using Microsoft.Win32;

using XsltEditor.Services.Abstractions;

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

		return dialog.ShowDialog() is true ? dialog.FileName : null;
	}

	public string? OpenSaveDialog(string title, params string[] extensions)
	{
		var dialog = new SaveFileDialog
		{
			Title = title,
			Filter = BuildFilter(title, extensions)
		};

		return dialog.ShowDialog() is true ? dialog.FileName : null;
	}

	private static string BuildFilter(string title, string[]? extensions)
	{
		if (extensions is null or [])
		{
			return "All Files|*.*";
		}

		var filter = string.Join(";", extensions.Select(ext => $"*{ext}"));
		return $"{title}|{filter}";
	}
}