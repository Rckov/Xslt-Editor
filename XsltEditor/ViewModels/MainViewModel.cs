using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

using XsltEditor.Helpers;
using XsltEditor.Infrastructure;
using XsltEditor.Models.Messages;
using XsltEditor.Services.Interfaces;
using XsltEditor.Transform.Enums;
using XsltEditor.ViewModels.Base;
using XsltEditor.Views.Windows;

namespace XsltEditor.ViewModels;

public class MainViewModel : BaseViewModel, IDisposable
{
    private readonly IMessenger _messenger;
    private readonly IXmlTransformService _transformService;
    private readonly IWindowService _windowService;

    public ObservableCollection<DocumentViewModel> Documents { get; set; }

    public DocumentViewModel? ActiveDocument
    {
        get;
        set => Set(ref field, value);
    }

    public string? HtmlContent
    {
        get;
        set => Set(ref field, value);
    }

    public EngineType Engine
    {
        set
        {
            _transformService.Create(value);
        }
    }

    public ICommand? OpenFileCommand { get; private set; }
    public ICommand? SaveFileCommand { get; private set; }
    public ICommand? OpenCompletionWindowCommand { get; private set; }
    public ICommand? OpenCaretLineWindowCommand { get; private set; }
    public ICommand? OpenFileExplorerCommand { get; private set; }
    public ICommand? OpenSettingsWindowCommand { get; private set; }

    public MainViewModel(
        IMessenger messenger,
        IWindowService windowService,
        IThemeManager themeManager,
        IXmlTransformService transformService,
        ISettingsService settingsService)
    {
        _messenger = messenger;
        _messenger.Subscribe<EngineMessage>(OnEngineChanged);
        _windowService = windowService;
        _transformService = transformService;

        Engine = settingsService.Settings.Engine;

        Documents = [
            new DocumentViewModel(messenger) { Name = "XSL" },
            new DocumentViewModel(messenger) { Name = "XML" }
        ];

        themeManager.Apply(settingsService.Settings.Theme);
        InitializeEvents();
    }

    private void InitializeEvents()
    {
        foreach (var item in Documents)
        {
            item.PropertyChanged += OnTextPropertyChanged;
        }
    }

    protected override void InitializeCommands()
    {
        OpenFileCommand = new RelayCommand(OpenFile);
        SaveFileCommand = new RelayCommand(SaveFile);
        OpenCompletionWindowCommand = new RelayCommand(OpenCompletionWindow);
        OpenCaretLineWindowCommand = new RelayCommand(OpenCaretLineWindow);
        OpenFileExplorerCommand = new RelayCommand(OpenFileExplorer);
        OpenSettingsWindowCommand = new RelayCommand(OpenSettingWindow);
    }

    private async void OpenFile()
    {
        var path = Dialog.OpenFile("Open File", ".xsl", ".xslt", ".xml");

        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        var document = GetDocumentByExtension(path);

        try
        {
            if (document != null)
            {
                await document.OpenDocument(path);
                ActiveDocument = document;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void SaveFile()
    {
        if (ActiveDocument is null)
        {
            return;
        }

        var path = ActiveDocument.FilePath;

        if (string.IsNullOrEmpty(path))
        {
            path = Dialog.SaveFile("Save File " + ActiveDocument.Name, ".xsl", ".xslt", ".xml");

            if (string.IsNullOrEmpty(path))
            {
                return;
            }
        }

        try
        {
            await ActiveDocument.SaveDocument(path);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void OnTextPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(DocumentViewModel.Text))
        {
            return;
        }

        var xsl = GetDocumentByExtension(".xsl");
        var xml = GetDocumentByExtension(".xml");

        if (xsl == null || xml == null)
        {
            LogInfo("Transformation skipped: XSL or XML document not found");
            return;
        }

        if (string.IsNullOrWhiteSpace(xsl.Text) || string.IsNullOrWhiteSpace(xml.Text))
        {
            HtmlContent = string.Empty;
            return;
        }

        HtmlContent = await _transformService.TransformAsync(xsl.Text, xml.Text, xsl.FilePath);
    }

    private void OpenCompletionWindow()
    {
        _windowService.ShowDialogWindow<CompletionView>();
    }

    private void OpenCaretLineWindow()
    {
        _windowService.ShowDialogWindow<CaretLineView>();
    }

    private void OpenSettingWindow(object? parameter)
    {
        _windowService.ShowDialogWindow<SettingsView>();
    }

    private void OpenFileExplorer()
    {
        if (ActiveDocument?.FilePath is not null)
        {
            Process.Start("explorer.exe", $"/select,\"{ActiveDocument.FilePath}\"");
        }
    }

    private void OnEngineChanged(EngineMessage message)
    {
        Engine = message.EngineType;
    }

    private DocumentViewModel? GetDocumentByExtension(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLower();

        return extension switch
        {
            ".xsl" or ".xslt" => Documents.FirstOrDefault(d => d.Name == "XSL"),
            ".xml" => Documents.FirstOrDefault(d => d.Name == "XML"),
            _ => null
        };
    }

    public void Dispose()
    {
        _messenger.Unsubscribe<EngineMessage>(OnEngineChanged);

        foreach (var item in Documents)
        {
            item.Dispose();
        }

        Documents.Clear();
        GC.SuppressFinalize(this);
    }
}