using Microsoft.Win32;

using System.IO;

using XsltEditor.Models.Base;

namespace XsltEditor.Models;

internal class FileDocument(string extension) : ObservableObject
{
    private bool _hasChanged;

    public bool HasChanged
    {
        get => _hasChanged;
        set => Set(ref _hasChanged, value);
    }

    private string? _text;

    public string? Text
    {
        get => _text;
        set => HasChanged = Set(ref _text, value);
    }

    private string? _fileName;

    public string? FileName
    {
        get => _fileName;
        set => Set(ref _fileName, value);
    }

    public async void Open()
    {
        var dialog = new OpenFileDialog
        {
            Filter = $"{extension.ToUpper()} File (*.{extension})|*.{extension}"
        };

        if (dialog.ShowDialog() == true)
        {
            FileName = dialog.FileName;
            Text = await File.ReadAllTextAsync(dialog.FileName);
        }
    }

    public async void Save()
    {
        if (string.IsNullOrEmpty(FileName) || !File.Exists(FileName))
        {
            var dialog = new SaveFileDialog
            {
                Filter = $"{extension.ToUpper()} File (*.{extension})|*.{extension}"
            };

            if (dialog.ShowDialog() == true)
            {
                FileName = dialog.FileName;
            }
            else
            {
                return;
            }
        }

        await File.WriteAllTextAsync(FileName, Text);
        HasChanged = false;
    }
}