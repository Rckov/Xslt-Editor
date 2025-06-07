using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;

using XsltEditor.Extensions;
using XsltEditor.Models.Enums;
using XsltEditor.Services;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.ViewModels;

internal partial class MainViewModel : ObservableObject
{
    private const int DebounceMilliseconds = 500;

    private readonly ISettingsService _settingsService;
    private readonly IXmlTransformService _transformService;
    private readonly IThemeService _themeService;
    private readonly IWindowService _windowService;

    [ObservableProperty] private string? _htmlContent;
    [ObservableProperty] private DocumentViewModel? _activeDocument;

    private DispatcherTimer? _debounceTimer;

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

        var settings = settingsService.Settings;

        themeService.ChangeTheme(settings.Theme);
        transformService.CreateEngine(settings.Engine);

        Documents =
        [
            CreateDocument("XSL", DocumentType.Xsl),
            CreateDocument("XML", DocumentType.Xml)
        ];

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

    [RelayCommand]
    private async Task ChangeTheme()
    {
        var curTheme = _themeService.CurrentTheme;
        var newTheme = curTheme == ThemeType.Light ? ThemeType.Dark : ThemeType.Light;

        _themeService.ChangeTheme(newTheme);

        _settingsService.Settings.Theme = newTheme;
        await _settingsService.SaveSettings();
    }

    private DocumentViewModel CreateDocument(string name, DocumentType documentType)
    {
        var document = new DocumentViewModel(
            App.Services.GetRequiredService<IDialogService>(),
            App.Services.GetRequiredService<IFileOperationsService>())
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

    private void InitializeDebounceTimer()
    {
        _debounceTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(DebounceMilliseconds)
        };
        _debounceTimer.Tick += OnDebounceTimerTick;
    }

    private void Document_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(DocumentViewModel.Text) || _debounceTimer == null)
        {
            return;
        }

        _debounceTimer.Stop();
        _debounceTimer.Start();
    }

    private async void OnDebounceTimerTick(object? sender, EventArgs e)
    {
        try
        {
            _debounceTimer?.Stop();

            var xsl = Documents.GetDocument(DocumentType.Xsl);
            var xml = Documents.GetDocument(DocumentType.Xml);

            if (xsl == null || xml == null || string.IsNullOrWhiteSpace(xsl.Text) || string.IsNullOrWhiteSpace(xml.Text))
            {
                HtmlContent = string.Empty;
                return;
            }

            HtmlContent = await _transformService.TransformAsync(xsl.Text, xml.Text, xsl.FilePath);
        }
        catch
        {
            HtmlContent = "Error during transformation.";
        }
    }
}