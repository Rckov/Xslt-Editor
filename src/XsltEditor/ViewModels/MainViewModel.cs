using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;

using XsltEditor.Extensions;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.ViewModels;

internal partial class MainViewModel : ObservableObject
{
    private const int DEBOUNCE_MILLISECONDS = 500;

    private readonly IWindowService _windowService;
    private readonly ISettingsService _settingsService;
    private readonly IThemeService _themeService;
    private readonly IXmlTransformService _transformService;

    private DispatcherTimer? _debounceTimer;

    [ObservableProperty] private string? _htmlContent;
    [ObservableProperty] private DocumentViewModel? _activeDocument;

    public MainViewModel(
        ISettingsService settingsService,
        IXmlTransformService transformService,
        IThemeService themeService,
        IWindowService windowService)
    {
        _settingsService = settingsService;
        _transformService = transformService;
        _themeService = themeService;
        _windowService = windowService;

        Documents =
        [
            CreateDocument("XSL", DocumentType.XSL),
            CreateDocument("XML", DocumentType.XML)
        ];

        InitializeSettings();
        InitializeDebounceTimer();
    }

    public ObservableCollection<DocumentViewModel> Documents { get; }

    [RelayCommand]
    private void OpenSettingsView()
    {
        _windowService.ShowDialog<SettingsViewModel>();
    }

    [RelayCommand]
    private void OpenCaretView()
    {
        _windowService.ShowDialog<CaretViewModel>();
    }

    [RelayCommand]
    private void OpenCompletionView()
    {
        _windowService.ShowDialog<CompletionViewModel>();
    }

    [RelayCommand]
    private async Task SaveDocument(DocumentType documentType)
    {
        try
        {
            var document = Documents.FirstOrDefault(x => x.DocumentType == documentType);
            if (document != null)
            {
                await document.SaveDocumentCommand.ExecuteAsync(null);
            }
        }
        catch (Exception ex)
        {
            _windowService.ShowMessage(ex.Message, "Error Saving the Document");
        }
    }

    [RelayCommand]
    private async Task OpenDocument(DocumentType documentType)
    {
        try
        {
            var document = Documents.FirstOrDefault(x => x.DocumentType == documentType);
            if (document != null)
            {
                await document.OpenDocumentCommand.ExecuteAsync(null);
                ActiveDocument = document;
            }
        }
        catch (Exception ex)
        {
            _windowService.ShowMessage(ex.Message, "Error Opening the Document");
        }
    }

    private DocumentViewModel CreateDocument(string name, DocumentType documentType)
    {
        var document = new DocumentViewModel
        {
            Name = name,
            DocumentType = documentType
        };

        AttachDocumentEvents(document);
        return document;
    }

    private void AttachDocumentEvents(DocumentViewModel document)
    {
        document.PropertyChanged += Document_PropertyChanged;
    }

    private void DetachDocumentEvents(DocumentViewModel document)
    {
        document.PropertyChanged -= Document_PropertyChanged;
    }

    private void InitializeSettings()
    {
        var theme = _settingsService.GetTheme();
        _themeService.ChangeTheme(theme);

        var engine = _settingsService.GetEngine();
        _transformService.CreateEngine(engine);
    }

    private void InitializeDebounceTimer()
    {
        _debounceTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(DEBOUNCE_MILLISECONDS)
        };
        _debounceTimer.Tick += OnDebounceTimerTick;
    }

    private void Document_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(DocumentViewModel.Text) && _debounceTimer != null)
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }
    }

    private async void OnDebounceTimerTick(object? sender, EventArgs e)
    {
        _debounceTimer?.Stop();

        var xsl = Documents.GetDocument(DocumentType.XSL);
        var xml = Documents.GetDocument(DocumentType.XML);

        if (xsl == null || xml == null || string.IsNullOrWhiteSpace(xsl.Text) || string.IsNullOrWhiteSpace(xml.Text))
        {
            HtmlContent = string.Empty;
            return;
        }

        try
        {
            HtmlContent = await _transformService.TransformAsync(xsl.Text, xml.Text, xsl.FilePath);
        }
        catch
        {
            HtmlContent = "Error during transformation.";
        }
    }
}