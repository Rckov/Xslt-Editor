using System.ComponentModel;
using System.IO;
using System.Runtime.Versioning;
using System.Text;
using System.Windows;
using System.Windows.Input;

using XsltEditor.Core;
using XsltEditor.Models.Base;
using XsltEditor.Tools;

namespace XsltEditor.ViewModels;

[SupportedOSPlatform("windows")]
public class DocumentViewModel : ObservableObject
{
    private readonly string _extension;

    public DocumentViewModel(string extension)
    {
        _extension = extension;

        OpenCommand = new RelayCommand(OpenDocument);
        SaveCommand = new RelayCommand(SaveDocument);
        PropertyChanged += DocumentViewModel_PropertyChanged;
    }

    public string? FilePath { get; private set; }

    public string? Content
    {
        get;
        set => Set(ref field, value);
    }

    public bool HasChanged
    {
        get;
        set => Set(ref field, value);
    }

    public Encoding? Encoding
    {
        get;
        private set => Set(ref field, value);
    }

    public ICommand? OpenCommand { get; private set; }
    public ICommand? SaveCommand { get; private set; }

    private void DocumentViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Content))
        {
            HasChanged = true;
        }
    }

    private async void OpenDocument(object? obj)
    {
        var path = Dialog.OpenFile("File", _extension);

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

            Content = await streamReader.ReadToEndAsync();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void SaveDocument(object? obj)
    {
        var path = File.Exists(FilePath) ? FilePath : Dialog.SaveFile("File", _extension);

        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        try
        {
            await using var fileStream = new FileStream(path, FileMode.Create);
            await using var streamWriter = new StreamWriter(fileStream);

            await streamWriter.WriteAsync(Content);

            if (string.IsNullOrEmpty(FilePath))
            {
                FilePath = path;
                Encoding = streamWriter.Encoding;
            }

            HasChanged = false;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}