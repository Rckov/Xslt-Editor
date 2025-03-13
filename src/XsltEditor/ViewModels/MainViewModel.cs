using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

using XsltEditor.Helpers;
using XsltEditor.Infrastructure;
using XsltEditor.Models.Messages;
using XsltEditor.Services.Interfaces;
using XsltEditor.Transform.Enums;
using XsltEditor.ViewModels.Base;
using XsltEditor.Views.Windows;

namespace XsltEditor.ViewModels;

[SupportedOSPlatform("windows")]
public class MainViewModel : BaseViewModel, IDisposable
{
    private readonly IMessenger _messenger;
    private readonly IXmlTransformService _transformService;
    private readonly IWindowService _windowService;

    private DispatcherTimer? _debounceTimer;

    public MainViewModel(
        IMessenger messenger,
        IWindowService windowService,
        IThemeManager themeManager,
        IXmlTransformService transformService,
        ISettingsService settingsService,
        ICompletionDataService completionDataService)
    {
        _windowService = windowService;
        _transformService = transformService;

        _messenger = messenger;
        _messenger.Subscribe<EngineMessage>(OnEngineChanged);
        _messenger.Subscribe<RuntimeCompileMessage>(OnCompileChanged);

        Documents =
        [
            new DocumentViewModel(messenger, completionDataService) { Name = "XSL" },
            new DocumentViewModel(messenger) { Name = "XML" }
        ];

        Engine = settingsService.Settings.Engine;
        IsRuntimeCompile = settingsService.Settings.IsRuntimeCompile;

        themeManager.Apply(settingsService.Settings.Theme);

        InitializeEvents();
    }

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
        set => _transformService.Create(value);
    }

    public bool IsRuntimeCompile
    {
        get => !field;
        set
        {
            if (Set(ref field, value))
            {
                InitializeDebounceTimer(value);
            }
        }
    }

    public ICommand? OpenFileCommand { get; private set; }
    public ICommand? SaveFileCommand { get; private set; }
    public ICommand? OpenCompletionWindowCommand { get; private set; }
    public ICommand? OpenCaretLineWindowCommand { get; private set; }
    public ICommand? OpenFileExplorerCommand { get; private set; }
    public ICommand? OpenSettingsWindowCommand { get; private set; }
    public ICommand? CompileCommand { get; private set; }

    public void Dispose()
    {
        _debounceTimer?.Stop();
        _messenger.Unsubscribe<EngineMessage>(OnEngineChanged);
        _messenger.Unsubscribe<RuntimeCompileMessage>(OnCompileChanged);

        foreach (var doc in Documents.ToList())
        {
            doc.PropertyChanged -= OnTextPropertyChanged;
            doc.Dispose();
        }

        Documents.Clear();
        GC.SuppressFinalize(this);
    }

    private void InitializeEvents()
    {
        foreach (var item in Documents)
        {
            item.PropertyChanged += OnTextPropertyChanged;
        }
    }

    private void InitializeDebounceTimer(bool isEnable)
    {
        if (isEnable)
        {
            _debounceTimer ??= new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1),
            };

            _debounceTimer.Tick += DebounceTimer_Tick;
        }
        else
        {
            if (_debounceTimer != null)
            {
                _debounceTimer.Stop();
                _debounceTimer.Tick -= DebounceTimer_Tick;
            }
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
        CompileCommand = new RelayCommand(async () => await Compile());
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
            if (document == null)
            {
                return;
            }

            await document.OpenDocument(path);
            ActiveDocument = document;
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

    private async Task Compile()
    {
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

    private async void DebounceTimer_Tick(object? sender, EventArgs e)
    {
        if (_debounceTimer != null && _debounceTimer.IsEnabled)
        {
            _debounceTimer.Stop();
        }

        await Compile();
    }

    private void OnTextPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(DocumentViewModel.Text))
        {
            return;
        }

        if (_debounceTimer != null)
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }
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

    private void OnCompileChanged(RuntimeCompileMessage message)
    {
        IsRuntimeCompile = message.IsRuntime;
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
}
