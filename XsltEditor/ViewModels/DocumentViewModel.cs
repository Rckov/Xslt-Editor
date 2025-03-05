using ICSharpCode.AvalonEdit.Highlighting;

using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

using XsltEditor.Models.Base;
using XsltEditor.Helpers;

namespace XsltEditor.ViewModels;

public class DocumentViewModel : ObservableObject
{
    public string? Name
    {
        get;
        set => Set(ref field, value);
    }

    public string? FilePath
    {
        get;
        set => Set(ref field, value);
    }

    public bool? IsDirty
    {
        get;
        set => Set(ref field, value);
    }

    public bool IsReadOnly
    {
        get;
        set => Set(ref field, value);
    }

    public int Line
    {
        get;
        set => Set(ref field, value);
    }

    public int Column
    {
        get;
        set => Set(ref field, value);
    }

    public string? Text
    {
        get;
        set => Set(ref field, value);
    }

    public Encoding? Encoding
    {
        get;
        set => Set(ref field, value);
    }

    public IHighlightingDefinition? Highlighting
    {
        get;
        set => Set(ref field, value);
    }

    public DocumentViewModel(string name)
    {
        Name = name;
        ThemeManager.ThemeChanged += ThemeManager_ThemeChanged;
    }

    public async Task OpenDocument(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        try
        {
            await using var fileStream = new FileStream(path, FileMode.Open);
            using var streamReader = new StreamReader(fileStream, true);

            FilePath = path;
            Encoding = streamReader.CurrentEncoding;
            Text = await streamReader.ReadToEndAsync();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public async Task SaveDocument(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        try
        {
            await using var fileStream = new FileStream(path, FileMode.Create);
            await using var streamWriter = new StreamWriter(fileStream);

            await streamWriter.WriteAsync(Text);

            if (string.IsNullOrEmpty(FilePath))
            {
                FilePath = path;
                Encoding = streamWriter.Encoding;
            }

            IsDirty = false;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ThemeManager_ThemeChanged(ThemeType obj)
    {
        if (ThemeManager.CurrentHighlighting is { } highlighting)
        {
            Highlighting = highlighting;
        }
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName is nameof(Text))
        {
            IsDirty = true;
        }
    }
}