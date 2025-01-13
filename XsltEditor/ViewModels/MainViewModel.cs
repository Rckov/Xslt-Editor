using System.ComponentModel;
using System.IO;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Input;

using XsltEditor.Models;
using XsltEditor.Models.Base;
using XsltEditor.Services.Interfaces;
using XsltEditor.Tools.Commands;
using XsltEditor.Tools.Helpers;
using XsltEditor.Transform.Enums;
using XsltEditor.Views;

namespace XsltEditor.ViewModels;

[SupportedOSPlatform("windows")]
public class MainViewModel : ObservableObject
{
    private readonly IXmlTransformService _transformService;
    private readonly IWindowService _windowService;

    public MainViewModel(IWindowService windowService, ISettingsService settingsService, IXmlTransformService transformService)
    {
        _windowService = windowService;

        _transformService = transformService;
        _transformService.Create(EngineType.XslCompiledTransform);

        InitCommands();
        SubscribeEvents();

        Settings = settingsService.LoadSettings();
    }

    public string? HtmlText
    {
        get;
        set => Set(ref field, value);
    }

    public Settings Settings { get; }
    public TextDocument XslDocument { get; } = new(".xsl");
    public TextDocument XmlDocument { get; } = new(".xml");

    public ICommand? OpenFileCommand { get; private set; }
    public ICommand? SaveFileCommand { get; private set; }
    public ICommand? OpenSettingsCommand { get; private set; }
    public ICommand? OpenCompletionWindowCommand { get; private set; }

    private void InitCommands()
    {
        OpenFileCommand = new RelayCommand(OpenFile);
        SaveFileCommand = new RelayCommand(SaveFile);
        OpenSettingsCommand = new RelayCommand(OpenSettings);
        OpenCompletionWindowCommand = new RelayCommand(OpenCompletionWindow);
    }

    private void SubscribeEvents()
    {
        XslDocument.PropertyChanged += OnTextPropertyChanged;
        XmlDocument.PropertyChanged += OnTextPropertyChanged;
    }

    private async void OpenFile(object? parameter)
    {
        if (parameter is not string extension)
        {
            return;
        }

        var filePath = FileDialog.OpenFile("File", extension);

        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        try
        {
            var load = extension.ToLowerInvariant() switch
            {
                ".xsl" => XslDocument.LoadContent(filePath),
                ".xml" => XmlDocument.LoadContent(filePath),
                _ => Task.CompletedTask
            };

            await load;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void SaveFile(object? parameter)
    {
        if (parameter is not TextDocument document)
        {
            return;
        }

        var path = File.Exists(document.FilePath) ? document.FilePath : FileDialog.SaveFile("File", document.Extension);

        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        try
        {
            await document.SaveContent(path);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OpenSettings(object? parameter)
    {
        _windowService.ShowDialogWindow<SettingsView>();
    }

    private void OpenCompletionWindow(object? parameter)
    {
        _windowService.ShowDialogWindow<CompletionView>();
    }

    private async void OnTextPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(TextDocument.Content))
        {
            return;
        }

        XslDocument.HasChanged = true;

        if (string.IsNullOrWhiteSpace(XslDocument.Content) || string.IsNullOrWhiteSpace(XmlDocument.Content))
        {
            HtmlText = string.Empty;
            return;
        }

        HtmlText = await _transformService.TransformAsync(XslDocument, XmlDocument);
    }
}