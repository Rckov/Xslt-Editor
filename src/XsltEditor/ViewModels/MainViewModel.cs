using System.ComponentModel;
using System.Windows.Input;

using XsltEditor.Commands;
using XsltEditor.Models;
using XsltEditor.Models.Base;
using XsltEditor.Services;

namespace XsltEditor.ViewModels;

internal class MainViewModel : ObservableObject
{
    private string? _htmlText;

    private FileDocument _xslFile = new("xsl");
    private FileDocument _xmlFile = new("xml");

    private readonly TransformService _transform = new();

    public MainViewModel()
    {
        SaveCommand = new SaveRelayCommand();
        OpenFileCommand = new OpenFileRelayCommand();
        OpenFolderCommand = new OpenFolderRelayCommand();

        XSL.PropertyChanged += OnTextPropertyChanged;
        XML.PropertyChanged += OnTextPropertyChanged;
    }

    public FileDocument XSL
    {
        get => _xslFile;
        set => Set(ref _xslFile, value);
    }

    public FileDocument XML
    {
        get => _xmlFile;
        set => Set(ref _xmlFile, value);
    }

    public string? HtmlText
    {
        get => _htmlText;
        set => Set(ref _htmlText, value);
    }

    public ICommand SaveCommand { get; }
    public ICommand OpenFileCommand { get; }
    public ICommand OpenFolderCommand { get; }

    private async void OnTextPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Text")
        {
            if (string.IsNullOrWhiteSpace(XML.Text) || string.IsNullOrWhiteSpace(XSL.Text))
            {
                HtmlText = string.Empty;
                return;
            }

            HtmlText = await _transform.TransformAsync(XML, XSL);
        }
    }
}